using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.SubServices.Abstracts;

public interface ISubServiceRepository
{
    Task<SubService> AddAsync(SubService entity, CancellationToken cancellationToken = default);
    Task<SubService?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<SubService?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken, bool asNoTracking = true);
    IQueryable<SubService> GetByMainServiceIdAndUser(Guid mainServiceId, Guid userId, bool asNoTracking = true);
    IQueryable<SubService> GetListByMainServiceIdWithDetails(Guid mainServiceId, bool asNoTracking = true);
    Task<IEnumerable<SubService>> GetListByMainServiceIdAsync(Guid mainServiceId, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalPrice(Guid mainServiceId, CancellationToken cancellationToken = default);
    Task<(decimal?, decimal?, decimal?)> GetTotalCost(Guid mainServiceId, CancellationToken cancellationToken = default);
}
