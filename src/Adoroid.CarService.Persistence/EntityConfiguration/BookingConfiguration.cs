using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.MobileUserId).IsRequired();
        builder.Property(b => b.CompanyId).IsRequired();
        builder.Property(b => b.BookingDate).IsRequired();
        builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Description).HasMaxLength(500);
        builder.Property(b => b.VehicleBrand).IsRequired().HasMaxLength(50);
        builder.Property(b => b.VehicleModel).IsRequired().HasMaxLength(30);
        builder.Property(b => b.VehicleYear).IsRequired();

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.MobileUser)
            .WithMany(b => b.Bookings)
            .HasForeignKey(b => b.MobileUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Company)
            .WithMany(b => b.Bookings)
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
