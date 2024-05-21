using TravelAgency.FleetService.API.Infrastructure.Interfaces;
using TravelAgency.FleetService.API.Infrastructure.Services;

namespace TravelAgency.FleetService.API.Configurations;

public static class ServicesConfigurations
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
