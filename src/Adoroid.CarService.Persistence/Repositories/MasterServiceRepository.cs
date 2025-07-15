using Adoroid.CarService.Application.Features.MasterServices.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class MasterServiceRepository(CarServiceDbContext dbContext) : IMasterServiceRepository
{
    public async Task AddAsync(MasterService masterService, CancellationToken cancellationToken = default)
    {
        await dbContext.MasterServices.AddAsync(masterService, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
       return await dbContext.MasterServices
            .AsNoTracking()
            .AnyAsync(x => x.ServiceName == name, cancellationToken);
    }

    public async Task<IEnumerable<MasterService>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.MasterServices
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<MasterService?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken = default)
    {
       var query = asNoTracking ? 
            dbContext.MasterServices.AsNoTracking() :
            dbContext.MasterServices;

        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<MasterService> GetQueryable()
    {
        return dbContext.MasterServices.AsNoTracking().AsQueryable();
    }
}
