using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.MainServices.Abstracts;

public interface IMainServiceRepository
{
    Task<MainService?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<MainService> AddAsync(MainService entity, CancellationToken cancellationToken = default);
    Task<MainService?> GetByIdWithVehiclesAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);
    IQueryable<MainService> GetByVehicleIdWithUser(Guid vehicleId, Guid userId, bool asNoTracking = true);
    IQueryable<MainService> GetByCompanyIdWithVehicle(Guid companyId, bool asNoTracking = true);
}
