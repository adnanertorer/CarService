using Adoroid.CarService.Application.Features.Vehicles.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

internal class VehicleUserRepository(CarServiceDbContext dbContext) : IVehicleUserRepository
{
    public async Task AddAsync(VehicleUser vehicleUser, CancellationToken cancellationToken = default)
    {
        await dbContext.VehicleUsers.AddAsync(vehicleUser, cancellationToken);
    }

    public IQueryable<VehicleUser> GetQueryable()
    {
        return dbContext.VehicleUsers
            .AsNoTracking();
    }

    public async Task<IEnumerable<VehicleUser>> GetVehicleUsersByType(Guid vehicleId, int type, CancellationToken cancellationToken = default)
    {
        return await dbContext.VehicleUsers
            .Where(i => i.VehicleId == vehicleId && i.UserTypeId == type)
            .ToListAsync(cancellationToken);
    }
}
