using Microsoft.AspNetCore.Mvc;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Features.ExpenseTypes.Get;

public sealed record GetExpenseTypeRequest([FromRoute]int Id) : IRequest<GetExpenseTypeResponse>;

public sealed record GetExpenseTypeResponse(int Id, string Name);

public sealed class GetExpenseTypeEndpoint : Endpoint<GetExpenseTypeRequest, GetExpenseTypeResponse>
{
    private readonly ISender _sender;

    public GetExpenseTypeEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/api/expense-types/{id}");
        AllowAnonymous();
        Options(x => x.WithTags("Expense Types"));
        Description(b => b
           .ProducesProblemDetails(StatusCodes.Status400BadRequest, "application/json+problem")
           .ProducesProblemFE<InternalErrorResponse>(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(GetExpenseTypeRequest req, CancellationToken ct)
    {
        var response = await _sender.Send(req);

        await SendAsync(response, StatusCodes.Status201Created);
    }
}

public sealed class GetExpenseTypeHandler : IRequestHandler<GetExpenseTypeRequest, GetExpenseTypeResponse>
{
    private readonly IExpenseTypeRepository _repository;

    public GetExpenseTypeHandler(IExpenseTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetExpenseTypeResponse> Handle(GetExpenseTypeRequest request, CancellationToken cancellationToken)
    {
        var type = await _repository.GetByIdAsync(request, cancellationToken);

        Guard.Against.Null(type);

        return type.Adapt<GetExpenseTypeResponse>();
    }
}

public sealed class GetExpenseTypeRequestValidator : Validator<GetExpenseTypeRequest>
{
    public GetExpenseTypeRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}   