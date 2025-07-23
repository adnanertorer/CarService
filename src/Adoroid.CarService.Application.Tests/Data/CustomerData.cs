using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Tests.Data;

public static class CustomerData
{
    
    public static List<Customer> Customers = [
        new Customer
        {
            Address = "Atatürk Mah. No:34",
            CompanyId = CompanyData.Company.Id,
            CreatedBy = UserData.User.Id,
            CreatedDate = DateTime.UtcNow,
            Email = "ayse.karaca@example.com",
            IsActive = true,
            Name = "Ayşe",
            Phone = "+90 534 123 4567",
            Surname = "Karaca",
            TaxNumber = "1234567890",
            TaxOffice = "Kadıköy Vergi Dairesi",
            IsDeleted = false,
            CityId = 41,
            DistrictId = 409
        }
    ];
}
