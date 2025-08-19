using Adoroid.CarService.Application.Common.Enums;

namespace Adoroid.CarService.Application.Features.MainServices.Dtos;

public class MainServiceDto
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string? Description { get; set; }
    public decimal? MaterialCost { get; set; }
    public decimal Cost { get; set; }
    public MainServiceStatusEnum MainServiceStatus { get; set; }
    public decimal? Kilometer { get; set; }

    public VehicleDto? Vehicle { get; set; }
}
