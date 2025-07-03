using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class VehicleUserConfiguration : IEntityTypeConfiguration<VehicleUser>
{
    public void Configure(EntityTypeBuilder<VehicleUser> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.VehicleId).IsRequired();
        builder.Property(b => b.UserId).IsRequired();

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.Vehicle)
            .WithMany(b => b.VehicleUsers)
            .HasForeignKey(b => b.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.User)
            .WithMany(b => b.VehicleUsers)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);
    }
}
