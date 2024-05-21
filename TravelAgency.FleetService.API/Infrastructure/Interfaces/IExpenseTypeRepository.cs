using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.ExpenseTypes.Create;
using TravelAgency.FleetService.API.Features.ExpenseTypes.Get;

namespace TravelAgency.FleetService.API.Infrastructure.Interfaces;

public interface IExpenseTypeRepository
{
    Task<ExpenseType> CreateAsync(CreateExpenseTypeRequest request, CancellationToken cancellationToken);
    Task<ExpenseType?> GetByIdAsync(GetExpenseTypeRequest request, CancellationToken cancellationToken);
}
