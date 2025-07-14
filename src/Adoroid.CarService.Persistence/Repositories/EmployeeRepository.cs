using Adoroid.CarService.Application.Features.Employees.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class EmployeeRepository(CarServiceDbContext dbContext) : IEmployeeRepository
{
    public async Task<Employee> AddAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Employees.AddAsync(entity, cancellationToken);
        return entity;
    }
    public async Task<Employee?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
            dbContext.Employees.AsNoTracking() :
            dbContext.Employees.AsQueryable();
        return await query.FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }
    public async Task<bool> AnyAsync(string name, string surname, Guid companyId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .AnyAsync(i => i.Name == name && i.Surname == surname && i.CompanyId == companyId, cancellationToken);
    }

    public IQueryable<Employee> GetAll(Guid companyId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
             dbContext.Employees.AsNoTracking() :
             dbContext.Employees.AsQueryable();

        return query.Where(i => i.CompanyId == companyId);
    }
}