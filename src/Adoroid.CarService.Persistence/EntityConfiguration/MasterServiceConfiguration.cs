using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoroid.CarService.Persistence.EntityConfiguration;

public class MasterServiceConfiguration : IEntityTypeConfiguration<MasterService>
{
    public void Configure(EntityTypeBuilder<MasterService> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.ServiceName).IsRequired().HasMaxLength(150);
        builder.Property(b => b.OrderIndex).IsRequired();

        builder.Property(i => i.CreatedDate).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(64);

        builder.Property(c => c.UpdatedBy).HasMaxLength(64);
        builder.Property(c => c.DeletedBy).HasMaxLength(64);

        builder.HasQueryFilter(c => c.IsDeleted == false || c.DeletedDate == null);

        builder.HasData(new List<MasterService> {
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 0,
                ServiceName = "Motor Tamir/Bakım"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 1,
                ServiceName = "Oto Elektrik"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 2,
                ServiceName = "Kaporta/Boya"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 3,
                ServiceName = "Isıtma/Klima"
            },
             new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 4,
                ServiceName = "Lastik Tamir/Değişim"
            },
             new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 5,
                ServiceName = "Oto Cam"
            },
              new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 6,
                ServiceName = "Egzoz"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 7,
                ServiceName = "Elektronik"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 8,
                ServiceName = "Lpg Montaj/Dönüşüm"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 9,
                ServiceName = "Ekspertiz"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 10,
                ServiceName = "Oto Döşeme"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 11,
                ServiceName = "Oto Modifiye"
            },
            new()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                OrderIndex = 12,
                ServiceName = "Oto Temizlik"
            }
        });
    }
}
