using Microsoft.EntityFrameworkCore;
using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.Vehicles.Create;
using TravelAgency.FleetService.API.Features.Vehicles.Get;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly IFleetServiceDbContext _context;

    public VehicleRepository(IFleetServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle> CreateAsync(CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = request.Adapt<Vehicle>();

        await _context.Vehicle.AddAsync(vehicle, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return vehicle;
    }

    public async Task<Vehicle?> GetByIdAsync(GetVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicle.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return vehicle;
    }
}
