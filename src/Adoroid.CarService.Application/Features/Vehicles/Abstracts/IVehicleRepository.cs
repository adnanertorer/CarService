using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Vehicles.Abstracts;

public interface IVehicleRepository
{
    Task<bool> ExistsAsync(string plate, string serialNumber, CancellationToken cancellationToken = default);
    Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByIdWithMainServiceAsync(Guid id, bool asNoTracking = true, 
        CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByIdAsync(Guid id, bool asNoTracking = true, 
        CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByIdWithVehicleUsersAsync(Guid id, bool asNoTracking = true,
        CancellationToken cancellationToken = default);
    Task<Vehicle?> GetBySerialNumber(string plate, string serialNumber, CancellationToken cancellationToken = default);
    IQueryable<Vehicle> GetAll(Guid companyId, Guid userId,
        CancellationToken cancellationToken = default);
    IQueryable<Vehicle> GetQueryable();
    IQueryable<Vehicle> GetVehiclesByUserIdAsync(Guid userId);
}
