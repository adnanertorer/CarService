using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Companies.Abstracts;

public interface ICompanyReposiyory
{
    Task<bool> IsCompanyExistsAsync(string taxNumber, string companyEmail, CancellationToken cancellationToken);
    Task AddAsync(Company company, CancellationToken cancellationToken);
    Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Company?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);
    IQueryable<Company> GetAllWithIncludes();
}
