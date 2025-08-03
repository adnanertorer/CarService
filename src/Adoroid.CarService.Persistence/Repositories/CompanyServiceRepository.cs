using Adoroid.CarService.Application.Features.CompanyMasterServices.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class CompanyServiceRepository(CarServiceDbContext dbContext) : ICompanyServiceRepository
{
    public async Task AddAsync(CompanyService entity, CancellationToken cancellationToken = default)
    {
        await dbContext.CompanyServices.AddAsync(entity, cancellationToken);
    }

    public async Task<CompanyService?> GetById(Guid id, bool asNoTracking, CancellationToken cancellationToken = default)
    {
        if (asNoTracking)
        {
            return await dbContext.CompanyServices
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        return await dbContext.CompanyServices
                 .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<CompanyService?> GetByIdWithSubTables(Guid id, bool asNoTracking, CancellationToken cancellationToken = default)
    {
        IQueryable<CompanyService> query;

        if (asNoTracking)
        {
            query = dbContext.CompanyServices
             .Include(i => i.MasterService)
             .Include(i => i.Company)
             .AsNoTracking()
             .AsQueryable();
        }
        else {
            query = dbContext.CompanyServices
             .Include(i => i.MasterService)
             .Include(i => i.Company)
             .AsQueryable();
        }

        return await query.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public IQueryable<CompanyService> GetMasterServices(CancellationToken cancellationToken = default)
    {
        return dbContext.CompanyServices
            .Include(i => i.MasterService)
            .Include(i => i.Company)
            .AsNoTracking();
    }

    public async Task<bool> IsExistAsync(Guid companyId, Guid masterServiceId, CancellationToken cancellationToken = default)
    {
        return await dbContext.CompanyServices
            .AsNoTracking()
            .AnyAsync(i => i.CompanyId == companyId && i.MasterServiceId == masterServiceId, cancellationToken);
    }
}
