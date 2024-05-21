using TravelAgency.FleetService.API.Domain.Common;

namespace TravelAgency.FleetService.API.Domain.Entities;

public sealed class Vehicle : BaseAuditableEntity
{
    public required string Made { get; set; }
    public required string Model { get; set; }
    public required string VIN { get; set; }

    public required int FleetId { get; set; }
    public required Fleet Fleet { get; set; }

    public required ICollection<Expense> Expenses { get; set; }
    public required ICollection<Insurance> Insurances { get; set; }
}
