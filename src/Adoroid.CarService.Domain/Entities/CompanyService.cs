using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class CompanyService : Entity<Guid>
{
    public Guid CompanyId { get; set; }
    public Guid MasterServiceId { get; set; }

    public Company Company { get; set; }
    public MasterService MasterService { get; set; }
}
