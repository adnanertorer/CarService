using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Customers.Abstracts;

public interface ICustomerRepository
{
    Task<bool> IsCustomerExistsAsync(Guid companyId, string phone, string? email,
        CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(Guid customerId, bool asNoTracking = true, 
        CancellationToken cancellationToken = default);

    Task<Customer?> GetByMobileUserIdAsync(Guid mobileUserId, bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task AddAsync(Customer customer, CancellationToken cancellationToken);

    Task<(Customer Customer, List<(VehicleUser VehicleUser, Vehicle Vehicle)> VehicleUsers)> 
        GetWithVehicleUsersAsync(Guid customerId, CancellationToken cancellationToken);

    IQueryable<Customer> GetAllWithIncludes(Guid companyId);
    Task<string> GetNameByIdAsync(Guid customerId, CancellationToken cancellationToken);
    IQueryable<Customer> GetByCompanyId(Guid companyId);
    Task<Dictionary<Guid, string>> GetCustomerNames(List<Guid> guids, CancellationToken cancellationToken);
}
