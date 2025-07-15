using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Abstracts;

public interface ICompanyServiceRepository
{
    Task<bool> IsExistAsync(Guid companyId, Guid masterServiceId, CancellationToken cancellationToken = default);
    Task AddAsync(CompanyService entity, CancellationToken cancellationToken = default);
    Task<CompanyService?> GetById(Guid id, bool asNoTracking, CancellationToken cancellationToken = default);
    Task<CompanyService?> GetByIdWithSubTables(Guid id, bool asNoTracking, CancellationToken cancellationToken = default);
    IQueryable<CompanyService> GetMasterServices(CancellationToken cancellationToken = default);
}
