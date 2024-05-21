using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.Vehicles.Create;
using TravelAgency.FleetService.API.Features.Vehicles.Get;

namespace TravelAgency.FleetService.API.Infrastructure.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle> CreateAsync(CreateVehicleRequest request, CancellationToken cancellationToken);
    Task<Vehicle?> GetByIdAsync(GetVehicleRequest request, CancellationToken cancellationToken);
}
