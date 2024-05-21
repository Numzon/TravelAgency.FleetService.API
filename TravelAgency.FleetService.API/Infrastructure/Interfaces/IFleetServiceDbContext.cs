using Microsoft.EntityFrameworkCore;
using TravelAgency.FleetService.API.Domain.Entities;

namespace TravelAgency.FleetService.API.Infrastructure.Interfaces;

public interface IFleetServiceDbContext
{
    public DbSet<ExpenseType> ExpenseType { get; set; }
    public DbSet<Fleet> Fleet { get; set; }
    public DbSet<Vehicle> Vehicle { get; set; }
    public DbSet<Expense> Expense { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
