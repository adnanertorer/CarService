namespace Adoroid.CarService.Application.Features.SubServices.Dtos;

public class MainServiceDto
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string? Description { get; set; }
    public decimal Cost { get; set; }
    public int MainServiceStatus { get; set; }
}
