using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.CustomerId).IsRequired();
        builder.Property(b => b.Brand).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Model).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Year).IsRequired();
        builder.Property(b => b.Plate).IsRequired().HasMaxLength(20);
        builder.Property(b => b.Engine).HasMaxLength(20);
        builder.Property(b => b.FuelTypeId).IsRequired();
        builder.Property(b => b.SerialNumber).HasMaxLength(30);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.Customer)
           .WithMany(c => c.Vehicles)
           .HasForeignKey(b => b.CustomerId)
           .OnDelete(DeleteBehavior.NoAction);
    }
}
