using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Customers.Abstracts;

public interface ICustomerRepository
{
    Task<bool> IsCustomerExistsAsync(string name, string surname, Guid companyId, string phone,
        CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(Guid customerId, bool asNoTracking = true, 
        CancellationToken cancellationToken);

    Task<Customer?> GetByMobileUserIdAsync(Guid mobileUserId, bool asNoTracking = true,
        CancellationToken cancellationToken);

    Task AddAsync(Customer customer, CancellationToken cancellationToken);

    Task<(Customer Customer, List<(VehicleUser VehicleUser, Vehicle Vehicle)> VehicleUsers)> 
        GetWithVehicleUsersAsync(Guid customerId, CancellationToken cancellationToken);

    IQueryable<Customer> GetAllWithIncludes(Guid companyId);
    Task<string> GetNameByIdAsync(Guid customerId, CancellationToken cancellationToken);
}
