using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.AuthorizedName).IsRequired().HasMaxLength(50);
        builder.Property(b => b.AuthorizedSurname).IsRequired().HasMaxLength(50);
        builder.Property(b => b.CityId).IsRequired();
        builder.Property(b => b.DistrictId).IsRequired();
        builder.Property(b => b.CompanyAddress).IsRequired().HasMaxLength(250);
        builder.Property(b => b.CompanyEmail).IsRequired().HasMaxLength(60);
        builder.Property(b => b.CompanyName).IsRequired().HasMaxLength(150);
        builder.Property(b => b.CompanyPhone).IsRequired().HasMaxLength(20);
        builder.Property(b => b.TaxNumber).HasMaxLength(16);
        builder.Property(b => b.TaxOffice).HasMaxLength(150);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);
    }
}
