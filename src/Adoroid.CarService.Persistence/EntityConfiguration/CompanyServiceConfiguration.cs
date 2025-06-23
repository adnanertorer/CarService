using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class CompanyServiceConfiguration : IEntityTypeConfiguration<CompanyService>
{
    public void Configure(EntityTypeBuilder<CompanyService> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.CompanyId).IsRequired();
        builder.Property(b => b.MasterServiceId).IsRequired();

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.Company)
          .WithMany(c => c.CompanyServices)
          .HasForeignKey(b => b.CompanyId)
          .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.MasterService)
          .WithMany(c => c.CompanyServices)
          .HasForeignKey(b => b.MasterServiceId)
          .OnDelete(DeleteBehavior.NoAction);
    }
}
