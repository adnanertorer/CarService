using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class User : Entity<Guid>
{
    public User()
    {
        UserToCompanies = new HashSet<UserToCompany>();
    }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? RefreshToken { get; set; }
    public string? OtpCode { get; set; }
    public DateTime? RefreshTokenExpr { get; set; }
    public bool? IsActive { get; set; }

    public ICollection<UserToCompany>? UserToCompanies { get; set; } 
}
