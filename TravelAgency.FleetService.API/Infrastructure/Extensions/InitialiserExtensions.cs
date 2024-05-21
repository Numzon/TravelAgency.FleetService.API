using TravelAgency.FleetService.API.Infrastructure.Persistance;

namespace TravelAgency.FleetService.API.Infrastructure.Extensions;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<FleetServiceDbContextInitialiser>();

        await initialiser.InitialiseAsync();
    }
}
