using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Commands.Update;

public record AdjustmentAccountTransactionCommand(Guid Id) : IRequest<Response<AccountTransactionDto>>;

public class AdjustmentAccountTransactionCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ILogger<AdjustmentAccountTransactionCommandHandler> logger)
    : IRequestHandler<AdjustmentAccountTransactionCommand, Response<AccountTransactionDto>>
{
    public async Task<Response<AccountTransactionDto>> Handle(AdjustmentAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.AccountingTransactions
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.NotFound);

        if(entity.MainServiceId is null)
            return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.MainServiceIdCannotBeNull);

        var mainService = await dbContext.MainServices
            .Include(i => i.Vehicle)
            .FirstOrDefaultAsync(e => e.Id == entity.MainServiceId, cancellationToken);

        if (mainService is null)
            return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.MainServiceNotFound);

        mainService.ServiceStatus = (int)MainServiceStatusEnum.Cancelled;

        var oldDebt = entity.Debt;
        var oldClaim = entity.Claim;
        decimal balance = 0;

        var accountOwnerId = entity.AccountOwnerId;
        var accountOwnerType = (AccountOwnerTypeEnum)entity.AccountOwnerType;



        var accountTransaction = new AccountingTransaction();
        accountTransaction.CompanyId = Guid.Parse(currentUser.CompanyId!);
        accountTransaction.CreatedBy = Guid.Parse(currentUser.Id!);
        accountTransaction.CreatedDate = DateTime.UtcNow;
        accountTransaction.AccountOwnerId = accountOwnerId;
        accountTransaction.AccountOwnerType = (int)accountOwnerType;
        accountTransaction.TransactionType = (int)TransactionTypeEnum.Adjustment;
        
        if(oldClaim > 0)
        {
            accountTransaction.Claim = oldClaim * -1;
            accountTransaction.Debt = 0;
            balance = await GetBalance(accountOwnerId, accountTransaction.Claim, cancellationToken);
        }
        else
        {
            accountTransaction.Claim = 0;
            accountTransaction.Debt = oldDebt * -1;
            balance = await GetBalance(accountOwnerId, accountTransaction.Debt, cancellationToken);
        }

        accountTransaction.Balance = balance;
        accountTransaction.Description = $"Hesap düzeltmesi. {mainService.Vehicle?.Plate} plaka numaralı araç için yapılan hizmet işlemi iptal edildi.";
        accountTransaction.TransactionDate = DateTime.UtcNow;
        accountTransaction.MainServiceId = mainService.Id;
        accountTransaction.IsDeleted = false;
        accountTransaction.AdjustedTransactionId = entity.Id;

        await dbContext.AccountingTransactions.AddAsync(accountTransaction, cancellationToken);

        string ownerName = string.Empty;

        await dbContext.SaveChangesAsync(cancellationToken);

        if (accountOwnerType == AccountOwnerTypeEnum.Customer)
        {
            var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == accountOwnerId, cancellationToken);
            if (customer != null)
            {
                ownerName = $"{customer.Name} {customer.Surname}";
            }
            else
            {
                logger.LogWarning("Customer with ID {AccountOwnerId} not found.", accountOwnerId);
                return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.CustomerNotFound);
            }
        }
        else if (accountOwnerType == AccountOwnerTypeEnum.MobileUser)
        {
            var mobileUser = await dbContext.MobileUsers.FirstOrDefaultAsync(m => m.Id == accountOwnerId, cancellationToken);
            if( mobileUser != null)
            {
                ownerName = $"{mobileUser.Name} {mobileUser.Surname}";
            }
            else
            {
                logger.LogWarning("Mobile User with ID {AccountOwnerId} not found.", accountOwnerId);
                return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.CustomerNotFound);
            }
        }

        logger.LogInformation("Adjustment transaction created successfully for MainServiceId: {MainServiceId}", mainService.Id);

        return Response<AccountTransactionDto>.Success(new AccountTransactionDto
        {
            AccountOwnerId = accountTransaction.AccountOwnerId,
            AccountOwnerType = accountTransaction.AccountOwnerType,
            Balance = accountTransaction.Balance,
            Claim = accountTransaction.Claim,
            CompanyId = accountTransaction.CompanyId,
            Debt = accountTransaction.Debt,
            Description = accountTransaction.Description,
            Id = accountTransaction.Id,
            OwnerName = ownerName,
            TransactionDate = accountTransaction.TransactionDate,
            TransactionType = accountTransaction.TransactionType
        });
    }

    private async Task<decimal> GetBalance(Guid customerId, decimal amount, CancellationToken cancellationToken)
    {
        var totalDebt = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == Guid.Parse(currentUser.CompanyId!))
            .SumAsync(i => i.Debt, cancellationToken);

        var totalClaim = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == Guid.Parse(currentUser.CompanyId!))
            .SumAsync(i => i.Claim, cancellationToken);

        return totalDebt - totalClaim + amount;
    }
}

