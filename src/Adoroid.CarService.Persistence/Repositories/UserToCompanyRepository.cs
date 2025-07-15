using Adoroid.CarService.Application.Features.UserToCompanies.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class UserToCompanyRepository(CarServiceDbContext dbContext) : IUserToCompanyRepository
{
    public async Task AddAsync(UserToCompany userToCompany, CancellationToken cancellationToken)
    {
        await dbContext.UserToCompanies.AddAsync(userToCompany, cancellationToken);
    }

    public async Task<UserToCompany?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
    {
        var qeury = asNoTracking
            ? dbContext.UserToCompanies.AsNoTracking()
            : dbContext.UserToCompanies;

        return await qeury.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<UserToCompany?> GetByIdWithIncludedAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
    {
        var qeury = asNoTracking
           ? dbContext.UserToCompanies.Include(e => e.Company)
            .Include(e => e.User)
            .AsNoTracking()
           : dbContext.UserToCompanies.Include(e => e.Company)
            .Include(e => e.User);

        return await qeury.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public IQueryable<UserToCompany> GetQueryable(Guid companyId)
    {
        return dbContext.UserToCompanies
            .Include(i => i.Company)
            .Include(i => i.User)
            .AsNoTracking()
            .Where(i => i.CompanyId == companyId);
    }

    public async Task<bool> IsExists(Guid userId, Guid companyId, CancellationToken cancellationToken)
    {
        return await dbContext.UserToCompanies
            .AsNoTracking()
            .AnyAsync(i => i.UserId == userId && i.CompanyId == companyId, cancellationToken);
    }
}
