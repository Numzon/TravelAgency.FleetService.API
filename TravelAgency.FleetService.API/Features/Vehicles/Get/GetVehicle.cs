using Microsoft.AspNetCore.Mvc;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Features.Vehicles.Get;

public sealed record GetVehicleRequest([FromRoute] int Id) : IRequest<GetVehicleResponse>;

public sealed record GetVehicleResponse(int Id, string Made, string Model, string VIN, int FleetId);

public sealed class GetVehicleEndpoint : Endpoint<GetVehicleRequest, GetVehicleResponse>
{
    private readonly ISender _sender;

    public GetVehicleEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/api/vehicles/{id}");
        AllowAnonymous();
        Options(x => x.WithTags("Vehicles"));
        Description(b => b
           .ProducesProblemDetails(StatusCodes.Status400BadRequest, "application/json+problem")
           .ProducesProblemFE<InternalErrorResponse>(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(GetVehicleRequest req, CancellationToken ct)
    {
        var response = await _sender.Send(req);

        await SendAsync(response, StatusCodes.Status201Created);
    }
}

public sealed class GetVehicleHandler : IRequestHandler<GetVehicleRequest, GetVehicleResponse>
{
    private readonly IVehicleRepository _repository;

    public GetVehicleHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetVehicleResponse> Handle(GetVehicleRequest request, CancellationToken cancellationToken)
    {
        var type = await _repository.GetByIdAsync(request, cancellationToken);

        Guard.Against.Null(type);

        return type.Adapt<GetVehicleResponse>();
    }
}

public sealed class GetVehicleRequestValidator : Validator<GetVehicleRequest>
{
    public GetVehicleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}