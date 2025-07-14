using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Users.Abtracts.Repositories;

namespace Adoroid.CarService.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarServiceDbContext _dbContext;
    public IUserRepository Users { get; }

    public UnitOfWork(CarServiceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Users = new UserRepository(_dbContext);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
