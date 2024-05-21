using TravelAgency.FleetService.API.Infrastructure.Interfaces;
using TravelAgency.FleetService.API.Infrastructure.Repositories;

namespace TravelAgency.FleetService.API.Configurations;
public static class RepositoriesConfiguration
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
        services.AddScoped<IFleetRepository, FleetRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();

        return services;
    }
}
