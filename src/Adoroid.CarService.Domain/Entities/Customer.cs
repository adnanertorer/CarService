using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class Customer : Entity<Guid>
{
    public Customer()
    {
        Vehicles = new HashSet<Vehicle>();
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

    public Company? Company { get; set; }
    public ICollection<Vehicle>? Vehicles { get; set; }
}
