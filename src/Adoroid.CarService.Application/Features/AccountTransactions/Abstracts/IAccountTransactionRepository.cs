using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Abstracts;

public interface IAccountTransactionRepository
{
    Task AddAsync(AccountingTransaction entity, CancellationToken cancellationToken);
    Task<decimal> GetBalanceAsync(Guid customerId, Guid companyId, CancellationToken cancellationToken);
    Task<AccountingTransaction?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<decimal> GetBalanceWithAmounAsync(Guid customerId, Guid companyId, decimal amount, CancellationToken cancellationToken);
    IQueryable<AccountingTransaction> GetByCompanyId(Guid companyId, bool asNoTracking = true);
}
