using Adoroid.CarService.Application.Features.Customers.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class CustomerRepository(CarServiceDbContext dbContext) : ICustomerRepository
{
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await dbContext.Customers.AddAsync(customer, cancellationToken);
    }

    public IQueryable<Customer> GetAllWithIncludes(Guid companyId)
    {
        return  dbContext.Customers
             .AsNoTracking()
             .Where(i => i.CompanyId == companyId && i.IsActive);
    }

    public async Task<Customer?> GetByIdAsync(Guid customerId, bool asNoTracking = true, CancellationToken cancellationToken)
    {
        var query = asNoTracking ? 
            dbContext.Customers.AsNoTracking() : 
            dbContext.Customers.AsQueryable();

        return await query
            .FirstOrDefaultAsync(predicate: c => c.Id == customerId, cancellationToken);
    }

    public async Task<Customer?> GetByMobileUserIdAsync(Guid mobileUserId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
           dbContext.Customers.AsNoTracking() :
           dbContext.Customers.AsQueryable();

        return await query
            .FirstOrDefaultAsync(predicate: c => c.MobileUserId == mobileUserId, cancellationToken);
    }

    public async Task<string> GetNameByIdAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .AsNoTracking()
            .Where(c => c.Id == customerId)
            .Select(c => new { c.Name, c.Surname })
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
            return $"{customer!.Name} {customer.Surname}";

        return "";
    }

    public async Task<(Customer Customer, List<(VehicleUser VehicleUser, Vehicle Vehicle)> VehicleUsers)> GetWithVehicleUsersAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var data = await dbContext.Customers
             .Where(c => c.Id == customerId)
             .Select(c => new
             {
                 Customer = c,
                 VehicleUsers = c.VehicleUsers
                     .Where(j => (c.MobileUserId != null && j.UserId == c.MobileUserId) || j.UserId == c.Id)
                     .Select(vu => new
                     {
                         VehicleUser = vu,
                         Vehicle = vu.Vehicle
                     }).ToList()
             })
             .FirstOrDefaultAsync(cancellationToken);

        if (data == null)
            return default;

        var result = data.VehicleUsers
            .Select(v => (v.VehicleUser, v.Vehicle))
            .ToList();

        return (data.Customer, result);
    }

    public async Task<bool> IsCustomerExistsAsync(string name, string surname, Guid companyId, string phone, CancellationToken cancellationToken)
    {
       return await dbContext.Customers.AsNoTracking()
            .AnyAsync(i => i.Name == name &&
            i.Surname == surname && i.Phone == phone &&
            i.CompanyId == companyId, cancellationToken);
    }
}
