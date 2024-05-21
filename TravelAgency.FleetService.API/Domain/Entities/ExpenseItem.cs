using TravelAgency.FleetService.API.Domain.Common;

namespace TravelAgency.FleetService.API.Domain.Entities;

public sealed class ExpenseItem : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required decimal Cost { get; set; }

    public required int ExpenseId { get; set; }
    public required Expense Expense { get; set; }
}
