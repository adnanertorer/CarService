namespace Adoroid.CarService.Application.Features.Users.Dtos;

public class CreateUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
}
