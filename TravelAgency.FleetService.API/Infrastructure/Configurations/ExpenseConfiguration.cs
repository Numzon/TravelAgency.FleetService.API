using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.FleetService.API.Domain.Entities;

namespace TravelAgency.FleetService.API.Infrastructure.Configurations;

public sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TransactionDate)
            .IsRequired();

        builder.HasOne(x => x.Vehicle)
            .WithMany(x => x.Expenses)
            .HasForeignKey(x => x.VehicleId)
            .IsRequired();

        builder.HasOne(x => x.ExpenseType)
            .WithMany(x => x.Expenses)
            .HasForeignKey(x => x.ExpenseTypeId)
            .IsRequired();
    }
}
