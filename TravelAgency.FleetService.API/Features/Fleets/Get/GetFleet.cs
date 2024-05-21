using Microsoft.AspNetCore.Mvc;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Features.Fleets.Get;

public sealed record GetFleetRequest([FromRoute]int Id) : IRequest<GetFleetResponse>;

public sealed record GetFleetResponse(int Id, string Name);

public sealed class GetFleetEndpoint : Endpoint<GetFleetRequest, GetFleetResponse>
{
    private readonly ISender _sender;

    public GetFleetEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/api/fleets/{id}");
        AllowAnonymous();
        Options(x => x.WithTags("Fleets"));
        Description(b => b
           .ProducesProblemDetails(StatusCodes.Status400BadRequest, "application/json+problem")
           .ProducesProblemFE<InternalErrorResponse>(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(GetFleetRequest req, CancellationToken ct)
    {
        var response = await _sender.Send(req);

        await SendAsync(response, StatusCodes.Status201Created);
    }
}

public sealed class GetFleetHandler : IRequestHandler<GetFleetRequest, GetFleetResponse>
{
    private readonly IFleetRepository _repository;

    public GetFleetHandler(IFleetRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetFleetResponse> Handle(GetFleetRequest request, CancellationToken cancellationToken)
    {
        var type = await _repository.GetByIdAsync(request, cancellationToken);

        Guard.Against.Null(type);

        return type.Adapt<GetFleetResponse>();
    }
}

public sealed class GetFleetRequestValidator : Validator<GetFleetRequest>
{
    public GetFleetRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}