using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class MobileUserConfiguration : IEntityTypeConfiguration<MobileUser>
{
    public void Configure(EntityTypeBuilder<MobileUser> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Surname).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Password).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Email).IsRequired().HasMaxLength(60);
        builder.Property(b => b.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(b => b.RefreshToken).HasMaxLength(150);
        builder.Property(b => b.OtpCode).HasMaxLength(6);
        builder.Property(c => c.Address).HasMaxLength(200);
        builder.Property(c => c.ProfilePicture).HasMaxLength(200);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(i => i.City)
            .WithMany(i => i.MobileUsers)
            .HasForeignKey(i => i.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.District)
            .WithMany(i => i.MobileUsers)
            .HasForeignKey(i => i.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
