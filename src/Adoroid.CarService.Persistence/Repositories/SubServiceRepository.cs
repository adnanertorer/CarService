using Adoroid.CarService.Application.Features.SubServices.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class SubServiceRepository(CarServiceDbContext dbContext) : ISubServiceRepository
{
    public async Task<SubService> AddAsync(SubService entity, CancellationToken cancellationToken = default)
    {
        await dbContext.SubServices.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<SubService?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
       var query = asNoTracking ?
            dbContext.SubServices.AsNoTracking() :
            dbContext.SubServices.AsQueryable();

        return await query.FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }

    public IQueryable<SubService> GetByMainServiceIdAndUser(Guid mainServiceId, Guid userId, bool asNoTracking = true)
    {
        var query = dbContext.SubServices
            .Where(i =>
                i.MainServiceId == mainServiceId &&
                i.MainService.Vehicle.VehicleUsers.Any(u => u.UserId == userId));

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<SubService?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken, bool asNoTracking = true)
    {
        var query = dbContext.SubServices
             .Include(i => i.MainService).ThenInclude(ms => ms.Vehicle)
             .Include(i => i.Employee)
             .Include(i => i.Supplier)
             .Where(i => i.Id == id);

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<SubService>> GetListByMainServiceIdAsync(Guid mainServiceId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking
            ? dbContext.SubServices.AsNoTracking()
            : dbContext.SubServices.AsQueryable();

        return await query.Where(i => i.MainServiceId == mainServiceId).ToListAsync(cancellationToken);
    }

    public IQueryable<SubService> GetListByMainServiceIdWithDetails(Guid mainServiceId, bool asNoTracking = true)
    {
        var query = dbContext.SubServices
           .Include(i => i.MainService).ThenInclude(ms => ms.Vehicle)
           .Include(i => i.Employee)
           .Include(i => i.Supplier)
           .Where(i => i.MainServiceId == mainServiceId);

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<decimal> GetTotalPrice(Guid mainServiceId, CancellationToken cancellationToken = default)
    {
        var costs = await dbContext.SubServices.AsNoTracking()
            .Where(i => i.MainServiceId == mainServiceId)
            .Select(i => i.Cost - (i.Discount ?? 0))
            .ToListAsync(cancellationToken);

        return costs.Sum();
    }

    public async Task<(decimal?, decimal?, decimal?, decimal?)> GetTotalCost(Guid mainServiceId, CancellationToken cancellationToken = default)
    {
        var totalMaterialCost = await dbContext.SubServices.AsNoTracking()
           .Where(i => i.MainServiceId == mainServiceId)
           .Select(i => i.MaterialCost)
           .SumAsync(cancellationToken);

        var totalCost = await dbContext.SubServices.AsNoTracking()
            .Where(i => i.MainServiceId == mainServiceId)
            .Select(i => i.Cost)
            .SumAsync(cancellationToken);

        var totalDiscount = await dbContext.SubServices.AsNoTracking()
           .Where(i => i.MainServiceId == mainServiceId && i.Discount != null)
           .Select(i => i.Discount)
           .SumAsync(cancellationToken);

        var netCost = await dbContext.SubServices.AsNoTracking()
            .Where(i => i.MainServiceId == mainServiceId)
            .Select(i => i.Cost - (i.Discount ?? 0))
            .SumAsync(cancellationToken);

        return (totalMaterialCost, totalCost, totalDiscount, netCost);
    }

    public async Task<decimal> GetTotalMaterialCost(Guid mainServiceId, CancellationToken cancellationToken = default)
    {
        var costs = await dbContext.SubServices.AsNoTracking()
            .Where(i => i.MainServiceId == mainServiceId)
            .Select(i => i.MaterialCost)
            .ToListAsync(cancellationToken);

        return costs.Sum(c => c ?? 0m);
    }
}
