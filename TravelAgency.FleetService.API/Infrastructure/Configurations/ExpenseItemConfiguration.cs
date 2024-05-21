using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.FleetService.API.Domain.Entities;

namespace TravelAgency.FleetService.API.Infrastructure.Configurations;

public sealed class ExpenseItemConfiguration : IEntityTypeConfiguration<ExpenseItem>
{
    public void Configure(EntityTypeBuilder<ExpenseItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Cost)
            .IsRequired();

        builder.HasOne(x => x.Expense)
            .WithMany(x => x.ExpenseItems)
            .HasForeignKey(x => x.ExpenseId)
            .IsRequired();
    }
}
