using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Vehicles.Abstracts;

public interface IVehicleUserRepository
{
    IQueryable<VehicleUser> GetQueryable();
    Task<IEnumerable<VehicleUser>> GetVehicleUsersByType(Guid vehicleId, int type, CancellationToken cancellationToken = default);
    Task AddAsync(VehicleUser vehicleUser, CancellationToken cancellationToken = default);
    Task<bool> IsVehicleNotTempoary(Guid vehicleId, CancellationToken cancellationToken = default);
}
