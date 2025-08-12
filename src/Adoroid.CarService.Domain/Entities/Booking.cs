using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class Booking : Entity<Guid>
{
    public Guid MobileUserId { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string VehicleBrand { get; set; }
    public string VehicleModel { get; set; }
    public int VehicleYear { get; set; }
    public int Status { get; set; }
    public string? CompanyMessage { get; set; }

    public MobileUser MobileUser { get; set; }
    public Company Company { get; set; }
}
