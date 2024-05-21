using TravelAgency.FleetService.API.Domain.Common;

namespace TravelAgency.FleetService.API.Domain.Entities;

public sealed class InsuranceType : LookupEntity
{
    public required ICollection<Insurance> Insurances { get; set; }
}
