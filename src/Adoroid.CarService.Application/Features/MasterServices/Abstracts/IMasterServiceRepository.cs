using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.MasterServices.Abstracts;

public interface IMasterServiceRepository
{
    Task AddAsync(MasterService masterService, CancellationToken cancellationToken = default);
    Task<MasterService?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<MasterService>> GetAllAsync(CancellationToken cancellationToken = default);
    IQueryable<MasterService> GetQueryable();
}
