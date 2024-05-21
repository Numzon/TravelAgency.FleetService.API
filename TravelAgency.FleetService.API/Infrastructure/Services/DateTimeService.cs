using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Infrastructure.Services;

public sealed class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
