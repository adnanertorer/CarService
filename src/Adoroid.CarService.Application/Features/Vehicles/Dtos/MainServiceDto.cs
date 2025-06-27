namespace Adoroid.CarService.Application.Features.Vehicles.Dtos;

public class MainServiceDto
{
    public Guid Id { get; set; }
    public DateTime ServiceDate { get; set; }
    public string? Description { get; set; }
    public decimal Cost { get; set; }

    public CompanyDto? Company { get; set; }
}
