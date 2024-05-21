using TravelAgency.FleetService.API.Domain.Common;

namespace TravelAgency.FleetService.API.Domain.Entities;

public class Expense : BaseAuditableEntity
{
    public required DateTime TransactionDate { get; set; }

    public required int ExpenseTypeId { get; set; }
    public required ExpenseType ExpenseType { get; set; }

    public required int VehicleId { get; set; }
    public required Vehicle Vehicle { get; set; }

    public required ICollection<ExpenseItem> ExpenseItems { get; set; }
}
