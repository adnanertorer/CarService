using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class Supplier : Entity<Guid>
{
    public Supplier()
    {
       SubServices = new HashSet<SubService>();
    }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public string? TaxNumber { get; set; }
    public string? TaxOffice { get; set; }

    public Company Company { get; set; }
    public City City { get; set; }
    public District District { get; set; }
    public ICollection<SubService>? SubServices { get; set; }
}
