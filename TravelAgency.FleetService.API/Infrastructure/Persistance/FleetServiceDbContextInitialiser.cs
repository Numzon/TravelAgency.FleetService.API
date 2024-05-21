using Microsoft.EntityFrameworkCore;
using Serilog;

namespace TravelAgency.FleetService.API.Infrastructure.Persistance;

public sealed class FleetServiceDbContextInitialiser
{
    private readonly FleetServiceDbContext _fleetServiceDbContext;

    public FleetServiceDbContextInitialiser(FleetServiceDbContext fleetServiceDbContext)
    {
        _fleetServiceDbContext = fleetServiceDbContext;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_fleetServiceDbContext.Database.IsNpgsql())
            {
                await _fleetServiceDbContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while initialising the database.", ex);
            throw;
        }
    }
}
