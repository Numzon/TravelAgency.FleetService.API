using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.Fleets.Create;
using TravelAgency.FleetService.API.Features.Fleets.Get;

namespace TravelAgency.FleetService.API.Infrastructure.Interfaces;

public interface IFleetRepository
{
    Task<Fleet> CreateAsync(CreateFleetRequest request, CancellationToken cancellationToken);
    Task<Fleet?> GetByIdAsync(GetFleetRequest request, CancellationToken cancellationToken);
}
