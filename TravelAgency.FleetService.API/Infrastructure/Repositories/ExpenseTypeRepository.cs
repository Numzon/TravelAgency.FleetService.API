using Microsoft.EntityFrameworkCore;
using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.ExpenseTypes.Create;
using TravelAgency.FleetService.API.Features.ExpenseTypes.Get;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Infrastructure.Repositories;

public sealed class ExpenseTypeRepository : IExpenseTypeRepository
{
    private readonly IFleetServiceDbContext _context;

    public ExpenseTypeRepository(IFleetServiceDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseType> CreateAsync(CreateExpenseTypeRequest request, CancellationToken cancellationToken)
    {
        var type = request.Adapt<ExpenseType>();

        await _context.ExpenseType.AddAsync(type, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return type;
    }

    public async Task<ExpenseType?> GetByIdAsync(GetExpenseTypeRequest request, CancellationToken cancellationToken)
    {
        var type = await _context.ExpenseType.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return type;
    }
}
