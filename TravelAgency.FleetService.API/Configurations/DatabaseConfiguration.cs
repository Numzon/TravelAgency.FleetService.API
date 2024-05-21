using TravelAgency.FleetService.API.Infrastructure.Interceptors;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;
using TravelAgency.FleetService.API.Infrastructure.Persistance;

namespace TravelAgency.FleetService.API.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services)
    {
        services.AddScoped<FleetServiceDbContextInitialiser>();
        services.AddScoped<IFleetServiceDbContext, FleetServiceDbContext>();
        services.AddScoped<BaseAuditableEntitySaveChangesInterceptor>();

        return services;
    }
}
