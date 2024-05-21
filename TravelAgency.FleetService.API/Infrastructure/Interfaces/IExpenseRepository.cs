using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.Expenses.Create;
using TravelAgency.FleetService.API.Features.Expenses.Get;

namespace TravelAgency.FleetService.API.Infrastructure.Interfaces;

public interface IExpenseRepository
{
    Task<Expense> CreateAsync(CreateExpenseRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<Expense>?> ListMatchingDateRangeAsync(ListExpenseRequest request, CancellationToken cancellationToken);
}
