using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Employees.Abstracts;

public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee entity, CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(string name, string surname, string? email, string phone, Guid companyId, CancellationToken cancellationToken = default);
    IQueryable<Employee> GetAll(Guid companyId, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<bool> IsExistingSameInfo(Guid companyId, string phone, string? email, Guid employeeId, CancellationToken cancellationToken = default);
}
