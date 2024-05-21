using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Features.Vehicles.Create;
public sealed record CreateVehicleRequest(string Made, string Model, string VIN, int FleetId) : IRequest<CreateVehicleResponse>;

public sealed record CreateVehicleResponse(int Id, string Made, string Model, string VIN, int FleetId);

public sealed class CreateVehicleEndpoint : Endpoint<CreateVehicleRequest, CreateVehicleResponse>
{
    private readonly ISender _sender;

    public CreateVehicleEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("/api/vehicles");
        AllowAnonymous();
        Options(x => x.WithTags("Vehicles"));
        Description(b => b
           .ProducesProblemDetails(StatusCodes.Status400BadRequest, "application/json+problem")
           .ProducesProblemFE<InternalErrorResponse>(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(CreateVehicleRequest req, CancellationToken ct)
    {
        var response = await _sender.Send(req);

        await SendAsync(response, StatusCodes.Status201Created);
    }
}

public sealed class CreateVehicleHandler : IRequestHandler<CreateVehicleRequest, CreateVehicleResponse>
{
    private readonly IVehicleRepository _repository;

    public CreateVehicleHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateVehicleResponse> Handle(CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        var type = await _repository.CreateAsync(request, cancellationToken);

        Guard.Against.Null(type);

        return type.Adapt<CreateVehicleResponse>();
    }
}

public sealed class CreateVehicleRequestValidator : Validator<CreateVehicleRequest>
{
    public CreateVehicleRequestValidator()
    {
        RuleFor(x => x.Made)
            .NotEmpty();

        RuleFor(x => x.Model)
            .NotEmpty();

        RuleFor(x => x.VIN)
            .NotEmpty();

        RuleFor(x => x.FleetId)
            .NotEmpty();
    }
}
