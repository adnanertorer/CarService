using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.MobileUser).IsRequired();
        builder.Property(b => b.VehicleId).IsRequired();
        builder.Property(b => b.MasterServiceId).IsRequired();
        builder.Property(b => b.CompanyId).IsRequired();
        builder.Property(b => b.BookingStartDate).IsRequired();
        builder.Property(b => b.BookingEndDate).IsRequired();
        builder.Property(b => b.BookingStatus).IsRequired();
        builder.Property(b => b.Description).HasMaxLength(500);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.MobileUser)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.MobileUserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Vehicle)
            .WithMany(v => v.Bookings)
            .HasForeignKey(b => b.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.MasterService)
            .WithMany(m => m.Bookings)
            .HasForeignKey(b => b.MasterServiceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Company)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
