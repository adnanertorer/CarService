using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Tests.Data;

public static class CompanyData
{
    public static readonly Company Company = new()
    {
        AuthorizedName = "Mehmet",
        AuthorizedSurname = "Yıldız",
        CityId = 41,
        CompanyAddress = "İnönü Cad. No:12, Merkez, Kocaeli",
        CompanyEmail = "info@ornekfirma.com",
        CompanyName = "Örnek Bilişim A.Ş.",
        CompanyPhone = "+90 212 555 4444",
        CreatedBy = Guid.NewGuid(),
        CreatedDate = DateTime.UtcNow,
        DistrictId = 409,
        IsDeleted = false,
        TaxNumber = "9876543210",
        TaxOffice = "Beşiktaş Vergi Dairesi",
        Id = Guid.Parse("e242b0b7-e829-408a-a962-d52af601f5e0")
    };
}
