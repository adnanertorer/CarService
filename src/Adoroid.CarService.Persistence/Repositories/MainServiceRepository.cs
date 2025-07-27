using Adoroid.CarService.Application.Features.MainServices.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class MainServiceRepository(CarServiceDbContext dbContext) : IMainServiceRepository
{
    public async Task<MainService> AddAsync(MainService entity, CancellationToken cancellationToken = default)
    {
        await dbContext.MainServices.AddAsync(entity, cancellationToken);
        return entity;
    }

    public IQueryable<MainService> GetByCompanyIdWithVehicle(Guid companyId, bool asNoTracking = true)
    {
        var query = dbContext.MainServices
             .Include(i => i.Vehicle)
                 .ThenInclude(v => v.VehicleUsers)
             .Where(i => i.Vehicle != null && i.CompanyId == companyId);

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<MainService?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
            dbContext.MainServices.AsNoTracking() :
            dbContext.MainServices.AsQueryable();

        return await query
            .Include(i => i.Vehicle)
            .FirstOrDefaultAsync(predicate: c => c.Id == id, cancellationToken);
    }

    public async Task<MainService?> GetByIdWithVehiclesAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        return await (asNoTracking ?
            dbContext.MainServices
            .Include(i => i.Vehicle)
            .ThenInclude(i => i.VehicleUsers)
            .AsNoTracking() :
            dbContext.MainServices.AsQueryable())
            .Include(i => i.Vehicle)
            .ThenInclude(i => i.VehicleUsers)
            .FirstOrDefaultAsync(predicate: c => c.Id == id, cancellationToken);
    }

    public IQueryable<MainService> GetByVehicleIdWithUser(Guid vehicleId, Guid userId, bool asNoTracking = true)
    {
        var query = dbContext.MainServices
            .Include(i => i.Vehicle)
            .ThenInclude(v => v!.VehicleUsers)
            .Where(i => i.VehicleId == vehicleId &&
                        i.Vehicle!.VehicleUsers.Any(vu => vu.UserId == userId));

        return asNoTracking ? query.AsNoTracking() : query;
    }
}
