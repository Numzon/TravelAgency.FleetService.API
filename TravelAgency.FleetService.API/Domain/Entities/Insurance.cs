using TravelAgency.FleetService.API.Domain.Common;
using TravelAgency.FleetService.API.Domain.ValueObject;

namespace TravelAgency.FleetService.API.Domain.Entities;

public sealed class Insurance : BaseAuditableEntity
{
    public required string Identifier { get; set; }
    public required DateTimeRange DateTimeRange { get; set; }

    public required int InsuranceTypeId { get; set; }
    public required InsuranceType InsuranceType { get; set; }

    public required int VehicleId { get; set; }
    public required Vehicle Vehicle { get; set; }
}
