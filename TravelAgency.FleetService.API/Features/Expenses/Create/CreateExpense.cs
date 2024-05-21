using TravelAgency.FleetService.API.Common.Models;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Features.Expenses.Create;

public sealed record CreateExpenseResponse(int Id, DateTime TransactionDate);

public sealed record CreateExpenseRequest(DateTime TransactionDate, int ExpenseTypeId, int VehicleId, IEnumerable<CreateExpenseItemDto> Items) : IRequest<CreateExpenseResponse>;


public sealed class CreateExpenseEndpoint : Endpoint<CreateExpenseRequest, CreateExpenseResponse>
{
    private readonly ISender _sender;

    public CreateExpenseEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("/api/expenses");
        AllowAnonymous();
        Options(x => x.WithTags("Expenses"));
        Description(b => b
           .ProducesProblemDetails(StatusCodes.Status400BadRequest, "application/json+problem")
           .ProducesProblemFE<InternalErrorResponse>(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(CreateExpenseRequest req, CancellationToken ct)
    {
        var response = await _sender.Send(req);

        await SendAsync(response, StatusCodes.Status201Created);
    }
}

public sealed class CreateExpenseHandler : IRequestHandler<CreateExpenseRequest, CreateExpenseResponse>
{
    private readonly IExpenseRepository _repository;

    public CreateExpenseHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateExpenseResponse> Handle(CreateExpenseRequest request, CancellationToken cancellationToken)
    {
        var expense = await _repository.CreateAsync(request, cancellationToken);

        Guard.Against.Null(expense);

        return expense.Adapt<CreateExpenseResponse>();
    }
}

public sealed class CreateExpenseRequestValidator : Validator<CreateExpenseRequest>
{
    public CreateExpenseRequestValidator()
    {
        RuleFor(x => x.TransactionDate)
            .NotEmpty();

        RuleFor(x => x.ExpenseTypeId)
            .NotEmpty();

        RuleFor(x => x.VehicleId)
            .NotEmpty();

        RuleForEach(x => x.Items)
            .NotEmpty()
            .SetValidator(new CreateExpenseItemDtoValidator());
    }
}

public sealed class CreateExpenseItemDtoValidator : Validator<CreateExpenseItemDto>
{
    public CreateExpenseItemDtoValidator()
    {
        RuleFor(x => x.Cost)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();
    }
}