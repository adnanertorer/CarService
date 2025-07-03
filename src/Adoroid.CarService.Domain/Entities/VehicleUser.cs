using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class VehicleUser : Entity<Guid>
{
    public Guid VehicleId { get; set; }
    public Guid UserId { get; set; }
    public int UserTypeId { get; set; }

    public Vehicle Vehicle { get; set; }
}
