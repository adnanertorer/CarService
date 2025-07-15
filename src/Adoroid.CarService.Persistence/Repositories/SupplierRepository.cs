using Adoroid.CarService.Application.Features.Suppliers.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class SupplierRepository(CarServiceDbContext dbContext) : ISupplierRepository
{
    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        await dbContext.Suppliers.AddAsync(supplier, cancellationToken);
    }

    public IQueryable<Supplier> GetAll(Guid companyId, CancellationToken cancellationToken = default)
    {
        return dbContext.Suppliers
            .AsNoTracking()
            .Where(i => i.CompanyId == companyId);
    }

    public Task<Supplier?> GetByIdAsync(string id, bool asNoTracking, CancellationToken cancellationToken)
    {
         var query = asNoTracking ?
            dbContext.Suppliers.AsNoTracking() :
            dbContext.Suppliers.AsQueryable();

        return query.FirstOrDefaultAsync(i => i.Id.ToString() == id, cancellationToken);
    }

    public async Task<bool> IsExist(string name, string contactName, string phoneNumber, CancellationToken cancellationToken)
    {
        return await dbContext.Suppliers
             .AsNoTracking()
             .AnyAsync(i => i.Name == name || i.ContactName == contactName || i.PhoneNumber == phoneNumber, cancellationToken);
    }
}
