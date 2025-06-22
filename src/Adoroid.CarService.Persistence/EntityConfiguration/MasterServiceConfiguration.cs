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
                CreatedBy = new Guid("7a3a0f64-b3d1-4c8e-bb44-7bcb3e2520a1"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 0,
                ServiceName = "Motor Tamir/Bakım",
                Id = new Guid("8f370c01-8ad7-4f6e-a427-62a0e6b8aa29")
            },
            new()
            {
                CreatedBy = new Guid("94df0ae0-45d4-4dcf-94a7-72b5116c4517"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 1,
                ServiceName = "Oto Elektrik",
                Id = new Guid("ce7e613a-6935-4370-b299-26fcd73f49db")
            },
            new()
            {
                CreatedBy = new Guid("a1c51c0d-4efb-4e11-94b8-041b7f57298e"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 2,
                ServiceName = "Kaporta/Boya",
                Id = new Guid("fa7cba68-8412-4cf5-a42b-0fa970902374")
            },
            new()
            {
                CreatedBy = new Guid("3df9ed8b-8dc0-4fa3-994e-bd19668f1971"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 3,
                ServiceName = "Isıtma/Klima",
                Id = new Guid("ca46e3f6-5e4e-4e61-9215-59f5dd8dd2f8")
            },
            new()
            {
                CreatedBy = new Guid("dd52404b-7d2e-4c9b-b462-16f75d76dc4e"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 4,
                ServiceName = "Lastik Tamir/Değişim",
                Id = new Guid("1c3d15c3-9446-406e-b1de-884cc78142ec")
            },
            new()
            {
                CreatedBy = new Guid("ba78e33b-75e5-4ac7-8715-04fbc0c6f07b"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 5,
                ServiceName = "Oto Cam",
                Id = new Guid("269358de-194a-4ce9-a93f-f68fd58b371d")
            },
            new()
            {
                CreatedBy = new Guid("b97c1cf4-6d68-4039-851a-0c00401713b2"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 6,
                ServiceName = "Egzoz",
                Id = new Guid("8ce99e0e-cb66-4ef1-b0cd-52ea11ea4620")
            },
            new()
            {
                CreatedBy = new Guid("61856049-c7d1-4eb8-8c9b-67894ad7dfe2"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 7,
                ServiceName = "Elektronik",
                Id = new Guid("a5c17247-10f4-4f59-a6e5-8df2030f5da3")
            },
            new()
            {
                CreatedBy = new Guid("b91d228f-0325-493e-bef3-01955cf00b95"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 8,
                ServiceName = "Lpg Montaj/Dönüşüm",
                Id = new Guid("2d310fa3-8e36-44d3-8df1-5a07adcb4d1e")
            },
            new()
            {
                CreatedBy = new Guid("f06e2b66-7cc3-4418-9c84-bb4e5072c87f"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 9,
                ServiceName = "Ekspertiz",
                Id = new Guid("88e1c1ae-ef3b-472a-bb61-c0e80f9c0a90")
            },
            new()
            {
                CreatedBy = new Guid("9cb23cb5-1761-4374-9479-55db33f14762"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 10,
                ServiceName = "Oto Döşeme",
                Id = new Guid("a7d2db3a-74e0-498b-8bce-7ed265e2e2fb")
            },
            new()
            {
                CreatedBy = new Guid("bff8ac59-f735-4cd4-bef6-6ec785c5a65a"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 11,
                ServiceName = "Oto Modifiye",
                Id = new Guid("e66fa2f6-1e7d-4c5d-98d7-eec49e5c8cbe")
            },
            new()
            {
                CreatedBy = new Guid("8dbdb3b5-4cc1-4aa0-9ed0-b6ea96a6797f"),
                CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false,
                OrderIndex = 12,
                ServiceName = "Oto Temizlik",
                Id = new Guid("1a572b68-67a9-4eb6-8136-176059fd1b70")
            }
        });
    }
}
