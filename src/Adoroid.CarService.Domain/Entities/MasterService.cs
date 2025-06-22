using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class MasterService : Entity<Guid>
{
    public MasterService()
    {
        CompanyServices = new HashSet<CompanyService>();
    }
    public string ServiceName { get; set; }
    public int OrderIndex { get; set; }

    public ICollection<CompanyService> CompanyServices { get; set; }
}
