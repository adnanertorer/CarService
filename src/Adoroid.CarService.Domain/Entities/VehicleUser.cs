using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class VehicleUser : Entity<Guid>
{
    public Guid VehicleId { get; set; }
    public Guid UserId { get; set; }
    public int UserTypeId { get; set; }
    public Guid? CustomerId { get; set; } // sadece EF'nin shadow property üretmemesi için tanımlandı, kullanılmaz!

    public Vehicle Vehicle { get; set; }
    public Customer? Customer {  get; set; }
}
