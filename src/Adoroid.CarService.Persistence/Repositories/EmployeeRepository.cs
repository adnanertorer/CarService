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
        if(asNoTracking)
        {
            return await dbContext.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
        }

        return await dbContext.Employees.FirstOrDefaultAsync(i => i.Id == id, cancellationToken: cancellationToken);
    }
    public async Task<bool> AnyAsync(string name, string surname, string? email, string phone, Guid companyId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .Where(i => i.Name == name && i.Surname == surname && i.CompanyId == companyId)
            .Where(c => c.PhoneNumber == phone || c.Email == email)
            .AnyAsync(cancellationToken);
    }

    public IQueryable<Employee> GetAll(Guid companyId, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (asNoTracking)
        {
            return dbContext.Employees
                .AsNoTracking()
                .Where(i => i.CompanyId == companyId).AsQueryable();
        }

        return dbContext.Employees.Where(i => i.CompanyId == companyId).AsQueryable();
    }

    public async Task<bool> IsExistingSameInfo(Guid companyId, string phone, string? email, Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Employees
            .AsNoTracking()
            .Where(c => c.CompanyId == companyId && c.IsActive)
            .Where(c => c.PhoneNumber == phone || c.Email == email)
            .Where(c => c.Id != employeeId)
            .AnyAsync(cancellationToken);
    }
}