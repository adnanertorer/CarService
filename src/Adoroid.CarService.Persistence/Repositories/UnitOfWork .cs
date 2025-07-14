using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.AccountTransactions.Abstracts;
using Adoroid.CarService.Application.Features.Companies.Abstracts;
using Adoroid.CarService.Application.Features.Customers.Abstracts;
using Adoroid.CarService.Application.Features.MainServices.Abstracts;
using Adoroid.CarService.Application.Features.SubServices.Abstracts;
using Adoroid.CarService.Application.Features.Users.Abtracts.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Adoroid.CarService.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarServiceDbContext _dbContext;
    private IDbContextTransaction? _transaction;
    public IUserRepository Users { get; }
    public ICompanyReposiyory Companies { get; }
    public IAccountTransactionRepository AccountTransactions { get; }

    public ICustomerRepository Customers { get; }

    public IMainServiceRepository MainServices { get; }

    public ISubServiceRepository SubServices { get; }

    public UnitOfWork(CarServiceDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        Users = new UserRepository(_dbContext);
        Companies = new CompanyRepository(_dbContext);
        AccountTransactions = new AccountTransactionRepository(_dbContext);
        Customers = new CustomerRepository(_dbContext);
        MainServices = new MainServiceRepository(_dbContext);
        SubServices = new SubServiceRepository(_dbContext);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction ??= await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}
