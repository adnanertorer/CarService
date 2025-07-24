using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired().HasMaxLength(250);
        builder.Property(b => b.ContactName).IsRequired().HasMaxLength(150);
        builder.Property(b => b.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(b => b.Email).HasMaxLength(60);
        builder.Property(b => b.Address).HasMaxLength(250);
        builder.Property(b => b.CompanyId).IsRequired();
        builder.Property(b => b.CityId).IsRequired();
        builder.Property(b => b.DistrictId).IsRequired();
        builder.Property(b => b.TaxNumber).HasMaxLength(20);
        builder.Property(b => b.TaxOffice).HasMaxLength(150);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.Company)
          .WithMany(c => c.Suppliers)
          .HasForeignKey(b => b.CompanyId)
          .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.City)
            .WithMany(c => c.Suppliers)
            .HasForeignKey(b => b.CityId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.District)
            .WithMany(c => c.Suppliers)
            .HasForeignKey(b => b.DistrictId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
