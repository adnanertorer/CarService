using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class SubServiceConfiguration : IEntityTypeConfiguration<SubService>
{
    public void Configure(EntityTypeBuilder<SubService> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.MainServiceId).IsRequired();
        builder.Property(b => b.Operation).IsRequired().HasMaxLength(150);
        builder.Property(b => b.EmployeeId).IsRequired();
        builder.Property(b => b.OperationDate).IsRequired();
        builder.Property(b => b.Description).HasMaxLength(250);
        builder.Property(b => b.Material).HasMaxLength(250);
        builder.Property(b => b.MaterialBrand).HasMaxLength(250);
        builder.Property(b => b.MaterialCost).HasPrecision(18, 2).HasDefaultValue(0);
        builder.Property(b => b.Discount).HasPrecision(18, 2).HasDefaultValue(0);
        builder.Property(b => b.Cost).IsRequired().HasPrecision(18,2);

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasOne(b => b.MainService)
            .WithMany(c => c.SubServices)
            .HasForeignKey(b => b.MainServiceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Employee)
           .WithMany(c => c.SubServices)
           .HasForeignKey(b => b.EmployeeId)
           .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Supplier)
            .WithMany(c => c.SubServices)
            .HasForeignKey(b => b.SupplierId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
