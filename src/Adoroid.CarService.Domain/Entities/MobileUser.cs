using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class MobileUser : Entity<Guid>
{
    public MobileUser()
    {
        Vehicles = new HashSet<Vehicle>();
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public string? OtpCode { get; init; }
    public DateTime? RefreshTokenExpr { get; set; }

     public ICollection<Vehicle>? Vehicles { get; set; }
}