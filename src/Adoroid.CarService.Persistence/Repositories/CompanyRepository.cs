using Adoroid.CarService.Application.Features.Companies.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class CompanyRepository(CarServiceDbContext dbContext) : ICompanyReposiyory
{
    public async Task AddAsync(Company company, CancellationToken cancellationToken)
    {
        await dbContext.Companies.AddAsync(company, cancellationToken);
    }

    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Companies.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<Company?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Companies
            .Include(i => i.City)
            .Include(i => i.District)
            .Include(i => i.CompanyServices).ThenInclude(i => i.MasterService)
            .AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<bool> IsCompanyExistsAsync(string taxNumber, string companyEmail, CancellationToken cancellationToken)
    {
        return await dbContext.Companies.AsNoTracking()
            .AnyAsync(x => x.TaxNumber == taxNumber || x.CompanyEmail == companyEmail, cancellationToken);
    }

    public IQueryable<Company> GetAllWithIncludes()
    {
        return dbContext.Companies
        .Include(i => i.City)
        .Include(i => i.District)
        .Include(i => i.CompanyServices).ThenInclude(i => i.MasterService)
        .AsNoTracking();
    }
}
