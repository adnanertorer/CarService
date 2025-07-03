using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class UserToCompany : Entity<Guid>
{
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; } 
    public int UserType { get; set; } 
    public User User { get; set; } 
    public Company Company { get; set; } 
}
