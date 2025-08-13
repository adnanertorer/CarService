using Adoroid.CarService.Application.Features.Customers.Abstracts;
using Adoroid.CarService.Application.Features.Customers.Dtos;
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
             .Include(i => i.City)
             .Include(i => i.District)
             .Include(i => i.VehicleUsers)
             .Where(i => i.CompanyId == companyId && i.IsActive);
    }

    public IQueryable<Customer> GetByCompanyId(Guid companyId)
    {
        return dbContext.Customers
            .AsNoTracking()
            .Where(c => c.CompanyId == companyId && c.IsActive);
    }

    public IQueryable<CustomerBasicInfoDto> GetBasicInfoByCompanyId(Guid companyId)
    {
        return dbContext.Customers
         .AsNoTracking()
         .Where(c => c.CompanyId == companyId)
         .OrderBy(c => c.Name).ThenBy(c => c.Surname)
         .Select(i => new CustomerBasicInfoDto
         {
             Id = i.Id,
             FullName = i.Name + " " + i.Surname
         });
    }

    public async Task<Customer?> GetByIdAsync(Guid customerId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (asNoTracking)
        {
            return await dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate: c => c.Id == customerId, cancellationToken);
        }

        return await dbContext.Customers
            .FirstOrDefaultAsync(predicate: c => c.Id == customerId, cancellationToken);
    }

    public async Task<Customer?> GetByIdWithIncludesAsync(Guid customerId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<Customer> query;

        if (asNoTracking)
        {
            query = dbContext.Customers
                .Include(i => i.City)
                .Include(i => i.District)
                .AsNoTracking()
                .AsQueryable();
        }
        else
        {
            query = dbContext.Customers
                .Include(i => i.City)
                .Include(i => i.District)
                .AsQueryable();
        }

        return await query
            .FirstOrDefaultAsync(predicate: c => c.Id == customerId, cancellationToken);
    }

    public async Task<Customer?> GetByMobileUserIdAsync(Guid mobileUserId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (asNoTracking)
        {
            return await dbContext.Customers.AsNoTracking()
                .FirstOrDefaultAsync(predicate: c => c.MobileUserId == mobileUserId, cancellationToken);
        }

        return await dbContext.Customers
            .FirstOrDefaultAsync(predicate: c => c.MobileUserId == mobileUserId, cancellationToken);
    }

    public async Task<Dictionary<Guid, string>> GetCustomerNames(List<Guid> guids, CancellationToken cancellationToken)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .Where(c => guids.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => $"{c.Name} {c.Surname}", cancellationToken);
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

    public async Task<bool> IsCustomerExistsAsync(Guid companyId, string phone, string? email, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(email))
        {
            email = email.Trim().ToLowerInvariant();
            return await dbContext.Customers.AsNoTracking()
                .AnyAsync(i => i.CompanyId == companyId && i.Email == email || i.Phone == phone, cancellationToken);
        }
        return await dbContext.Customers.AsNoTracking()
               .AnyAsync(i => i.CompanyId == companyId && i.Phone == phone, cancellationToken);

    }

    public async Task<bool> IsExistingSameInfo(Guid companyId, string phone, string? email, Guid? customerId = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .Where(c => c.CompanyId == companyId && c.IsActive)
            .Where(c => c.Phone == phone || c.Email == email)
            .Where(c => c.Id != customerId)
            .AnyAsync(cancellationToken);
    }
}
