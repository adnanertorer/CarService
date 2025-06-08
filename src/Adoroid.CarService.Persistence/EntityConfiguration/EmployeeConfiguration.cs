using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired().HasMaxLength(50);
        builder.Property(b => b.Surname).IsRequired().HasMaxLength(50);
        builder.Property(b => b.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(b => b.Email).HasMaxLength(60);
        builder.Property(b => b.Address).HasMaxLength(250);
        builder.Property(b => b.IsActive).IsRequired();
        builder.Property(b => b.CompanyId).IsRequired();

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);
    }
}
