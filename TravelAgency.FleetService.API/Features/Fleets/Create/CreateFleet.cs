using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Features.Fleets.Create;

public sealed record CreateFleetRequest(string Name) : IRequest<CreateFleetResponse>;

public sealed record CreateFleetResponse(int Id, string Name);

public sealed class CreateFleetEndpoint : Endpoint<CreateFleetRequest, CreateFleetResponse>
{
    private readonly ISender _sender;

    public CreateFleetEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("/api/fleets");
        AllowAnonymous();
        Options(x => x.WithTags("Fleets"));
        Description(b => b
           .ProducesProblemDetails(StatusCodes.Status400BadRequest, "application/json+problem")
           .ProducesProblemFE<InternalErrorResponse>(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(CreateFleetRequest req, CancellationToken ct)
    {
        var response = await _sender.Send(req);

        await SendAsync(response, StatusCodes.Status201Created);
    }
}

public sealed class CreateFleetHandler : IRequestHandler<CreateFleetRequest, CreateFleetResponse>
{
    private readonly IFleetRepository _repository;

    public CreateFleetHandler(IFleetRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateFleetResponse> Handle(CreateFleetRequest request, CancellationToken cancellationToken)
    {
        var type = await _repository.CreateAsync(request, cancellationToken);

        Guard.Against.Null(type);

        return type.Adapt<CreateFleetResponse>();
    }
}

public sealed class CreateFleetRequestValidator : Validator<CreateFleetRequest>
{
    public CreateFleetRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}
