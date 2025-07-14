using Adoroid.CarService.Application.Features.AccountTransactions.Abstracts;
using Adoroid.CarService.Application.Features.Companies.Abstracts;
using Adoroid.CarService.Application.Features.Customers.Abstracts;
using Adoroid.CarService.Application.Features.MainServices.Abstracts;
using Adoroid.CarService.Application.Features.SubServices.Abstracts;
using Adoroid.CarService.Application.Features.Users.Abtracts.Repositories;

namespace Adoroid.CarService.Application.Common.Abstractions;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ICompanyReposiyory Companies { get; }
    IAccountTransactionRepository AccountTransactions { get; }
    ICustomerRepository Customers { get; }
    IMainServiceRepository MainServices { get; }
    ISubServiceRepository SubServices { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}
