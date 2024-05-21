using TravelAgency.FleetService.API.Common.Models;
using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.Expenses.Create;
using TravelAgency.FleetService.API.Features.Vehicles.Create;
using TravelAgency.FleetService.API.Features.Vehicles.Get;

namespace TravelAgency.FleetService.API.Configurations;

public static class MapsterConfiguration
{
    public static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Vehicle, CreateVehicleResponse>
            .NewConfig()
            .Map(dest => dest.VIN, src => src.VIN);

        TypeAdapterConfig<Vehicle, GetVehicleResponse>
            .NewConfig()
            .Map(dest => dest.VIN, src => src.VIN);

        TypeAdapterConfig<CreateExpenseRequest, Expense>
            .NewConfig()
            .Map(dest => dest.ExpenseItems, src => src.Items);

        TypeAdapterConfig<Expense, ExpenseListItemDto>
            .NewConfig()
            .Map(dest => dest.Cost, src => src.ExpenseItems.Select(x => x.Cost).Sum());

        return services;
    }
}
