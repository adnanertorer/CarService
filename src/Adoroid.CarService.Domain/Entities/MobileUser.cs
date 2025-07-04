using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class MobileUser : Entity<Guid>
{

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public string? OtpCode { get; init; }
    public DateTime? RefreshTokenExpr { get; set; }
    public string? Address { get; set; }
    public string? ProfilePicture { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }

    public City City { get; set; }
    public District District { get; set; }
}