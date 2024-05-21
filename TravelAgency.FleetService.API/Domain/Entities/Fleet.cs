using TravelAgency.FleetService.API.Domain.Common;

namespace TravelAgency.FleetService.API.Domain.Entities;

public sealed class Fleet : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required ICollection<Vehicle> Vehicles { get; set; }
}
