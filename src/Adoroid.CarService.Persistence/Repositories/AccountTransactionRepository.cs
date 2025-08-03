using Adoroid.CarService.Application.Features.AccountTransactions.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class AccountTransactionRepository(CarServiceDbContext dbContext) : IAccountTransactionRepository
{
    public async Task AddAsync(AccountingTransaction entity, CancellationToken cancellationToken)
    {
        await dbContext.AccountingTransactions.AddAsync(entity, cancellationToken);
    }

    public async Task<decimal> GetBalanceAsync(Guid customerId, Guid companyId, CancellationToken cancellationToken)
    {
        var totalDebt = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == companyId)
            .SumAsync(i => i.Debt, cancellationToken);

        var totalClaim = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == companyId)
            .SumAsync(i => i.Claim, cancellationToken);

        return Math.Abs(totalDebt - totalClaim);
    }

    public async Task<decimal> GetBalanceWithAmounAsync(Guid customerId, Guid companyId, decimal amount, CancellationToken cancellationToken)
    {
        var totalDebt = await dbContext.AccountingTransactions.AsNoTracking()
           .Where(i => i.AccountOwnerId == customerId && i.CompanyId == companyId)
           .SumAsync(i => i.Debt, cancellationToken);

        var totalClaim = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == companyId)
            .SumAsync(i => i.Claim, cancellationToken);

        return totalDebt - totalClaim + amount;
    }

    public IQueryable<AccountingTransaction> GetByCompanyId(Guid companyId, bool asNoTracking = true)
    {
       return asNoTracking ? dbContext.AccountingTransactions
            .AsNoTracking() : dbContext.AccountingTransactions
            .Where(i => i.CompanyId == companyId);
    }

    public IQueryable<AccountingTransaction> GetByCustomerId(Guid companyId, Guid customerId, bool asNoTracking = true)
    {
        return asNoTracking ? dbContext.AccountingTransactions
             .AsNoTracking() : dbContext.AccountingTransactions
             .Where(i => i.CompanyId == companyId && i.AccountOwnerId == customerId);
    }

    public async Task<AccountingTransaction?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = dbContext.AccountingTransactions.AsQueryable();
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }
        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
