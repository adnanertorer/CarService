using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.CompanyId).IsRequired();
        builder.Property(b => b.Name).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Surname).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Password).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Email).IsRequired().HasMaxLength(60);
        builder.Property(b => b.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(b => b.RefreshToken).HasMaxLength(150);
        builder.Property(b => b.OtpCode).HasMaxLength(6);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
