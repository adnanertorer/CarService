using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class Booking : Entity<Guid>
{
    public Guid MobileUserId { get; set; } 
    public Guid VehicleId { get; set; }
    public Guid MasterServiceId { get; set; }
    public Guid CompanyId { get; set; } 
    public DateTime BookingStartDate { get; set; }
    public DateTime BookingEndDate { get; set; }
    public int BookingStatus { get; set; }
    public string? Description { get; set; }

    public MobileUser MobileUser { get; set; }
    public Vehicle Vehicle { get; set; }
    public MasterService MasterService { get; set; }
    public Company Company { get; set; }
}
