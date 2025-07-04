namespace Adoroid.CarService.Application.Features.Customers.Dtos;

public class VehicleUserDto
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Guid UserId { get; set; }
    public int UserTypeId { get; set; }

    public VehicleDto Vehicle { get; set; }
}
