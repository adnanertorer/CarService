using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class AccountingTransactionConfiguration : IEntityTypeConfiguration<AccountingTransaction>
{
    public void Configure(EntityTypeBuilder<AccountingTransaction> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.CustomerId).IsRequired();
        builder.Property(b => b.CompanyId).IsRequired();
        builder.Property(b => b.TransactionType).IsRequired();
        builder.Property(b => b.Claim).IsRequired().HasPrecision(18,2);
        builder.Property(b => b.Debt).IsRequired().HasPrecision(18, 2);
        builder.Property(b => b.Balance).IsRequired().HasPrecision(18, 2);
        builder.Property(b => b.TransactionDate).IsRequired();

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);
    }
}
