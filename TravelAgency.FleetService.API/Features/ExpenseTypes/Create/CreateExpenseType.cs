using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Features.ExpenseTypes.Create;

public sealed record CreateExpenseTypeRequest(string Name) : IRequest<CreateExpenseTypeResponse>;

public sealed record CreateExpenseTypeResponse(int Id, string Name);

public sealed class CreateExpenseTypeEndpoint : Endpoint<CreateExpenseTypeRequest, CreateExpenseTypeResponse>
{
    private readonly ISender _sender;

    public CreateExpenseTypeEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("/api/expense-types");
        AllowAnonymous();
        Options(x => x.WithTags("Expense Types"));
        Description(b => b
           .ProducesProblemDetails(StatusCodes.Status400BadRequest, "application/json+problem")
           .ProducesProblemFE<InternalErrorResponse>(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(CreateExpenseTypeRequest req, CancellationToken ct)
    {
        var response = await _sender.Send(req);

        await SendAsync(response, StatusCodes.Status201Created);
    }
}

public sealed class CreateExpenseTypeHandler : IRequestHandler<CreateExpenseTypeRequest, CreateExpenseTypeResponse>
{
    private readonly IExpenseTypeRepository _repository;

    public CreateExpenseTypeHandler(IExpenseTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateExpenseTypeResponse> Handle(CreateExpenseTypeRequest request, CancellationToken cancellationToken)
    {
        var type = await _repository.CreateAsync(request, cancellationToken);

        Guard.Against.Null(type);

        return type.Adapt<CreateExpenseTypeResponse>();
    }
}

public sealed class CreateExpenseTypeRequestValidator : Validator<CreateExpenseTypeRequest>
{
    public CreateExpenseTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}
