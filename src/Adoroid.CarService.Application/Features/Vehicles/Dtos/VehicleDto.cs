namespace Adoroid.CarService.Application.Features.Vehicles.Dtos;

public class VehicleDto
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Plate { get; set; }
    public string? Engine { get; set; }
    public int FuelTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public Guid? MobileUserId { get; set; }

    public CustomerDto? Customer { get; set; }
    public MobileUserDto? MobileUser { get; set; }
    public List<MainServiceDto>? MainServices { get; set; }
}
