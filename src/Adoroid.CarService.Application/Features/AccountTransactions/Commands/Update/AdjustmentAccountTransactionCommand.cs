using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;
using System.ComponentModel.Design;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Commands.Update;

public record AdjustmentAccountTransactionCommand(Guid Id) : IRequest<Response<AccountTransactionDto>>;

public class AdjustmentAccountTransactionCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser,
    ILogger<AdjustmentAccountTransactionCommandHandler> logger, ICacheService cacheService)
    : IRequestHandler<AdjustmentAccountTransactionCommand, Response<AccountTransactionDto>>
{
    const string redisKeyPrefix = "mainservice:list";
    public async Task<Response<AccountTransactionDto>> Handle(AdjustmentAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.AccountTransactions.GetByIdAsync(request.Id, false, cancellationToken);

        if (entity is null)
            return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.NotFound);

        if(entity.MainServiceId is null)
            return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.MainServiceIdCannotBeNull);

        var mainService = await unitOfWork.MainServices.GetByIdWithVehiclesAsync(entity.MainServiceId.Value, false, cancellationToken);

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
            balance = await unitOfWork.AccountTransactions.GetBalanceWithAmounAsync(accountOwnerId, Guid.Parse(currentUser.CompanyId!), accountTransaction.Claim, cancellationToken);
        }
        else
        {
            accountTransaction.Claim = 0;
            accountTransaction.Debt = oldDebt * -1;
            balance = await unitOfWork.AccountTransactions.GetBalanceWithAmounAsync(accountOwnerId, Guid.Parse(currentUser.CompanyId!), accountTransaction.Debt, cancellationToken);
        }

        accountTransaction.Balance = balance;
        accountTransaction.Description = $"Hesap düzeltmesi. {mainService.Vehicle?.Plate} plaka numaralı araç için yapılan hizmet işlemi iptal edildi.";
        accountTransaction.TransactionDate = DateTime.UtcNow;
        accountTransaction.MainServiceId = mainService.Id;
        accountTransaction.IsDeleted = false;
        accountTransaction.AdjustedTransactionId = entity.Id;

        await unitOfWork.AccountTransactions.AddAsync(accountTransaction, cancellationToken);

        string ownerName = string.Empty;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (accountOwnerType == AccountOwnerTypeEnum.Customer)
        {
            var customer = await unitOfWork.Customers.GetByIdAsync(accountOwnerId, true, cancellationToken);
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
            var mobileUser = await unitOfWork.Customers.GetByMobileUserIdAsync(accountOwnerId, true, cancellationToken);
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

      
        try
        {
            var mainServiceDto = mainService.FromEntity();
            await cacheService.UpdateToListAsync($"{redisKeyPrefix}:{Guid.Parse(currentUser.CompanyId!)}", request.Id.ToString(), mainServiceDto, null);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Cache güncelleme işlemi başarısız oldu. MainServiceId: {MainServiceId}";
            logger.LogError(ex, errorMessage, request.Id);
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
}

