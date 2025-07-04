using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class Customer : Entity<Guid>
{
    public Customer()
    {
        VehicleUsers = new HashSet<VehicleUser>();
    }

    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Email { get; set; }
    public string Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public string? TaxNumber { get; set; }
    public string? TaxOffice { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public Guid? MobileUserId { get; set; }

    public Company? Company { get; set; }
    public ICollection<VehicleUser>? VehicleUsers { get; set; }
}
