using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class Employee : Entity<Guid>
{
    public Employee()
    {
        SubServices = new HashSet<SubService>();
    }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }

    public Company? Company { get; set; }
    public ICollection<SubService>? SubServices { get; set; }
}
