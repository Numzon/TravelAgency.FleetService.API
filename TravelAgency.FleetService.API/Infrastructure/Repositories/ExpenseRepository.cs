using Microsoft.EntityFrameworkCore;
using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Features.Expenses.Create;
using TravelAgency.FleetService.API.Features.Expenses.Get;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly IFleetServiceDbContext _context;

    public ExpenseRepository(IFleetServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Expense> CreateAsync(CreateExpenseRequest request, CancellationToken cancellationToken)
    {
        var expense = request.Adapt<Expense>();

        await _context.Expense.AddAsync(expense, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return expense;
    }

    public async Task<IEnumerable<Expense>?> ListMatchingDateRangeAsync(ListExpenseRequest request, CancellationToken cancellationToken)
    {
        var expenses = await _context.Expense
            .Where(x => x.TransactionDate >= request.From && x.TransactionDate <= request.To)
            .Include(x => x.ExpenseItems)
            .ToListAsync(cancellationToken);

        return expenses;
    }
}
