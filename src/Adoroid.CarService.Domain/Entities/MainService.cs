using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class MainService : Entity<Guid>
{
    public MainService()
    {
        SubServices = new HashSet<SubService>();
    }
    public Guid VehicleId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string? Description { get; set; }
    public decimal Cost { get; set; }
    public int ServiceStatus { get; set; }
    public Guid CompanyId { get; set; }

    public Vehicle? Vehicle { get; set; }
    public Company? Company { get; set; }
    public ICollection<SubService>? SubServices { get; set; }
}
