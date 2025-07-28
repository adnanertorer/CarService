namespace Adoroid.CarService.Application.Features.Bookings.Dtos;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid MobileUserId { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string VehicleBrand { get; set; }
    public string VehicleModel { get; set; }
    public int VehicleYear { get; set; }
    public bool IsApproved { get; set; }

    public CompanyDto? Company { get; set; }
    public MobileUserDto? MobileUser { get; set; }
}
