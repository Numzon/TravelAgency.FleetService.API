using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.FleetService.API.Domain.Entities;

namespace TravelAgency.FleetService.API.Infrastructure.Configurations;

public sealed class InsuranceConfiguration : IEntityTypeConfiguration<Insurance>
{
    public void Configure(EntityTypeBuilder<Insurance> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Identifier)
            .IsRequired();

        builder.OwnsOne(x => x.DateTimeRange);

        builder.HasOne(x => x.InsuranceType)
            .WithMany(x => x.Insurances)
            .HasForeignKey(x => x.InsuranceTypeId)
            .IsRequired();

        builder.HasOne(x => x.Vehicle)
            .WithMany(x => x.Insurances)
            .HasForeignKey(x => x.VehicleId)
            .IsRequired();
    }
}
