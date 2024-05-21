using TravelAgency.FleetService.API.Common.Models;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Features.Expenses.Get;

public sealed record ListExpenseRequest(DateTime From, DateTime To) : IRequest<ListExpenseResponse>;

public sealed record ListExpenseResponse(IEnumerable<ExpenseListItemDto> Expenses);

public sealed class GetExpenseEndpoint : Endpoint<ListExpenseRequest, ListExpenseResponse>
{
    private readonly ISender _sender;

    public GetExpenseEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/api/expenses");
        AllowAnonymous();
        Options(x => x.WithTags("Expenses"));
        Description(b => b
           .ProducesProblemDetails(StatusCodes.Status400BadRequest, "application/json+problem")
           .ProducesProblemFE<InternalErrorResponse>(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(ListExpenseRequest req, CancellationToken ct)
    {
        var response = await _sender.Send(req);

        await SendAsync(response, StatusCodes.Status201Created);
    }
}

public sealed class ListExpenseHandler : IRequestHandler<ListExpenseRequest, ListExpenseResponse>
{
    private readonly IExpenseRepository _repository;

    public ListExpenseHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<ListExpenseResponse> Handle(ListExpenseRequest request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.ListMatchingDateRangeAsync(request, cancellationToken);

        Guard.Against.Null(expenses);

        var expensesDto = expenses.Adapt<IEnumerable<ExpenseListItemDto>>();

        return new ListExpenseResponse(expensesDto);
    }
}

public sealed class ListExpenseRequestValidator : Validator<ListExpenseRequest>
{
    public ListExpenseRequestValidator()
    {
        RuleFor(x => x.From)
            .NotEmpty();

        RuleFor(x => x.To)
            .NotEmpty();
    }
}