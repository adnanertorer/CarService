using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class UserToCompanyConfiguration : IEntityTypeConfiguration<UserToCompany>
{
    public void Configure(EntityTypeBuilder<UserToCompany> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.UserId).IsRequired();
        builder.Property(i => i.CompanyId).IsRequired();
        builder.Property(i => i.UserType).IsRequired();

        builder.HasOne(i => i.User)
            .WithMany(i => i.UserToCompanies)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Company)
            .WithMany(i => i.UserToCompanies)
            .HasForeignKey(i => i.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);
    }
}
