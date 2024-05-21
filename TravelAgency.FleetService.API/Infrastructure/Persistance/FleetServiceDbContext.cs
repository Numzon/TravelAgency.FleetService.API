using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TravelAgency.FleetService.API.Domain.Entities;
using TravelAgency.FleetService.API.Infrastructure.Extensions;
using TravelAgency.FleetService.API.Infrastructure.Interceptors;
using TravelAgency.FleetService.API.Infrastructure.Interfaces;

namespace TravelAgency.FleetService.API.Infrastructure.Persistance;

public class FleetServiceDbContext : DbContext, IFleetServiceDbContext
{
    private readonly IMediator _mediator;
    private readonly BaseAuditableEntitySaveChangesInterceptor _baseAuditableEntitySaveChangesInterceptor;

    public DbSet<ExpenseType> ExpenseType { get; set; }
    public DbSet<Fleet> Fleet { get; set; }
    public DbSet<Vehicle> Vehicle { get; set; }
    public DbSet<Expense> Expense { get; set; }

    public FleetServiceDbContext(DbContextOptions<FleetServiceDbContext> options,
        IMediator mediator,
        BaseAuditableEntitySaveChangesInterceptor baseAuditableEntitySaveChangesInterceptor) : base(options)
    {
        _mediator = mediator;
        _baseAuditableEntitySaveChangesInterceptor = baseAuditableEntitySaveChangesInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_baseAuditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
