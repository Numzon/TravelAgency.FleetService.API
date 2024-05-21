namespace TravelAgency.FleetService.API.Common.Models;

public sealed record ExpenseListItemDto(int Id, DateTime TransactionDate, decimal Cost);