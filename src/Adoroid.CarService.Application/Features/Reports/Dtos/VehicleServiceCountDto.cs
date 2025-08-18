namespace Adoroid.CarService.Application.Features.Reports.Dtos;

public class VehicleServiceCountDto
{
    public Guid VehicleId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int ServiceCount { get; set; } = 0;
}
