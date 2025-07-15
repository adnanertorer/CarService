using Adoroid.CarService.Application.Features.Vehicles.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class VehicleRepository(CarServiceDbContext dbContext) : IVehicleRepository
{
    public async Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
        return vehicle;
    }

    public async Task<bool> ExistsAsync(string plate, string serialNumber, CancellationToken cancellationToken = default)
    {
       return await dbContext.Vehicles
            .AsNoTracking()
            .AnyAsync(i => i.Plate == plate && i.SerialNumber == serialNumber, cancellationToken);
    }

    public IQueryable<Vehicle> GetAll(Guid companyId, Guid userId, CancellationToken cancellationToken = default)
    {
        var query = from cus in dbContext.Customers
                    from vu in dbContext.VehicleUsers
                    where cus.CompanyId == companyId && (cus.Id == vu.UserId || cus.MobileUserId == vu.UserId)
                    join veh in dbContext.Vehicles on vu.VehicleId equals veh.Id into vehJoin
                    from veh in vehJoin.DefaultIfEmpty()
                    select veh;

        return query;
    }

    public IQueryable<Vehicle> GetQueryable()
    {
        return dbContext.Vehicles
            .AsNoTracking();
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
            dbContext.Vehicles.AsNoTracking() : dbContext.Vehicles;

        return await query
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<Vehicle?> GetByIdWithMainServiceAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
       var query = asNoTracking ?
            dbContext.Vehicles.AsNoTracking() : dbContext.Vehicles;

        return await query
            .Include(i => i.MainServices)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<Vehicle?> GetByIdWithVehicleUsersAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
            dbContext.Vehicles.AsNoTracking() : dbContext.Vehicles;

        return await query
            .Include(i => i.VehicleUsers)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<Vehicle?> GetBySerialNumber(string plate, string serialNumber, CancellationToken cancellationToken = default)
    {
        return await dbContext.Vehicles
             .AsNoTracking()
             .FirstOrDefaultAsync(i => i.Plate == plate && i.SerialNumber == serialNumber, cancellationToken);
    }

    public IQueryable<Vehicle> GetVehiclesByUserIdAsync(Guid userId)
    {
        return dbContext.Vehicles
           .Include(i => i.MainServices).ThenInclude(i => i.SubServices)
           .Include(i => i.MainServices).ThenInclude(i => i.Company)
           .Where(i => i.VehicleUsers.Any(i => i.UserId == userId))
           .AsNoTracking();
    }
}
