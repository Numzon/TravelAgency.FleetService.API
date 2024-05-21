using Microsoft.EntityFrameworkCore;
using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.Fleets.Create;
using TravelAgency.FleetService.API.Features.Fleets.Get;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Infrastructure.Repositories;

public sealed class FleetRepository : IFleetRepository
{
    private readonly IFleetServiceDbContext _context;

    public FleetRepository(IFleetServiceDbContext context)
	{
        _context = context;
    }

    public async Task<Fleet> CreateAsync(CreateFleetRequest request, CancellationToken cancellationToken)
    {
        var fleet = request.Adapt<Fleet>();

        await _context.Fleet.AddAsync(fleet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return fleet;
    }

    public async Task<Fleet?> GetByIdAsync(GetFleetRequest request, CancellationToken cancellationToken)
    {
        var fleet = await _context.Fleet.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return fleet;
    }
}
