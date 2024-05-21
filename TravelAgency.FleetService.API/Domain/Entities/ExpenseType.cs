using TravelAgency.FleetService.API.Domain.Common;

namespace TravelAgency.FleetService.API.Domain.Entities;

public sealed class ExpenseType : LookupEntity
{
    public required ICollection<Expense> Expenses { get; set; }
}
