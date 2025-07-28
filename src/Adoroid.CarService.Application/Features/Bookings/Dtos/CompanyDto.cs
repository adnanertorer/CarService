namespace Adoroid.CarService.Application.Features.Bookings.Dtos;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string AuthorizedPerson { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
