using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Abstracts;

public interface IUserToCompanyRepository
{
    Task AddAsync(UserToCompany userToCompany, CancellationToken cancellationToken);
    Task<bool> IsExists(Guid userId, Guid companyId, CancellationToken cancellationToken);
    Task<UserToCompany?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken);
    Task<UserToCompany?> GetByIdWithIncludedAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken);
    IQueryable<UserToCompany> GetQueryable(Guid companyId);
}
