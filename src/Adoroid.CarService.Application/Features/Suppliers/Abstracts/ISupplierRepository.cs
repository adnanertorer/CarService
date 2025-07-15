using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Suppliers.Abstracts;

public interface ISupplierRepository
{
    Task<bool> IsExist(string name, string contactName, string phoneNumber, CancellationToken cancellationToken);
    Task<Supplier?> GetByIdAsync(string id, bool asNoTracking, CancellationToken cancellationToken);
    Task AddAsync(Supplier supplier, CancellationToken cancellationToken);
    IQueryable<Supplier> GetAll(Guid companyId, CancellationToken cancellationToken = default);
}
