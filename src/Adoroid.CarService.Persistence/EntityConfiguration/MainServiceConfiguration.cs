using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class MainServiceConfiguration : IEntityTypeConfiguration<MainService>
{
    public void Configure(EntityTypeBuilder<MainService> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.VehicleId).IsRequired();
        builder.Property(b => b.ServiceStatus).IsRequired();
        builder.Property(b => b.ServiceDate).IsRequired();
        builder.Property(b => b.Description).HasMaxLength(250);
        builder.Property(b => b.Cost).IsRequired().HasPrecision(18, 2);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.Vehicle)
        .WithMany(c => c.MainServices)
        .HasForeignKey(b => b.VehicleId)
        .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Company)
            .WithMany(c => c.MainServices)
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
