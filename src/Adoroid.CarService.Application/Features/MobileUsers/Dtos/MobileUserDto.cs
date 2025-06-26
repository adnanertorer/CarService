namespace Adoroid.CarService.Application.Features.MobileUsers.Dtos;

public class MobileUserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
}
