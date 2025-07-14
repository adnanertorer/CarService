using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.AccountTransactions.Abstracts;
using Adoroid.CarService.Application.Features.Companies.Abstracts;
using Adoroid.CarService.Application.Features.Customers.Abstracts;
using Adoroid.CarService.Application.Features.Users.Abtracts.Repositories;

namespace Adoroid.CarService.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarServiceDbContext _dbContext;
    public IUserRepository Users { get; }
    public ICompanyReposiyory Companies { get; }
    public IAccountTransactionRepository AccountTransactions { get; }

    public ICustomerRepository Customers { get; }

    public UnitOfWork(CarServiceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Users = new UserRepository(_dbContext);
        Companies = new CompanyRepository(_dbContext);
        AccountTransactions = new AccountTransactionRepository(_dbContext);
        Customers = new CustomerRepository(_dbContext);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
