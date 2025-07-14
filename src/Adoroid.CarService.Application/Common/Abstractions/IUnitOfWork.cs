using Adoroid.CarService.Application.Features.Users.Abtracts.Repositories;

namespace Adoroid.CarService.Application.Common.Abstractions;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
