using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Update;

public record UpdateMainServiceCommand(Guid Id, Guid VehicleId, DateTime ServiceDate, string? Description, MainServiceStatusEnum ServiceStatus)
    : IRequest<Response<MainServiceDto>>;

public class UpdateMainServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ICacheService cacheService)
        : IRequestHandler<UpdateMainServiceCommand, Response<MainServiceDto>>
{
    const string redisKeyPrefix = "mainservice:list";
    public async Task<Response<MainServiceDto>> Handle(UpdateMainServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.MainServices
            .Include(i => i.Vehicle)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        entity.ServiceDate = request.ServiceDate;
        entity.Description = request.Description;
        entity.VehicleId = request.VehicleId;
        entity.ServiceStatus = (int)request.ServiceStatus;

        entity.UpdatedBy = Guid.Parse(currentUser.Id!);
        entity.UpdatedDate = DateTime.UtcNow;

        if(request.ServiceStatus == MainServiceStatusEnum.Done)
        {
            entity.Cost = await GetTotalPrice(request.Id, cancellationToken);

            if(entity.Vehicle is null)
                return Response<MainServiceDto>.Fail(BusinessExceptionMessages.VehicleNotFound);

            decimal balance = 0;

            if (entity.Vehicle.CustomerId != null)
                    balance = await GetBalance(entity.Vehicle!.CustomerId.Value, cancellationToken);

            else if (entity.Vehicle.MobileUserId != null)
                    balance = await GetBalance(entity.Vehicle!.MobileUserId.Value, cancellationToken);

            var accountTransaction = new AccountingTransaction();
            accountTransaction.Balance = balance + entity.Cost;
            accountTransaction.Claim = 0;
            accountTransaction.CompanyId = Guid.Parse(currentUser.CompanyId!);
            accountTransaction.CreatedBy = Guid.Parse(currentUser.Id!);
            accountTransaction.CreatedDate = DateTime.UtcNow;

            if (entity.Vehicle.CustomerId != null)
            {
                accountTransaction.AccountOwnerId = entity.Vehicle.CustomerId.Value;
                accountTransaction.AccountOwnerType = (int)AccountOwnerTypeEnum.Customer;
            }
            else if (entity.Vehicle.MobileUserId != null)
            {
                accountTransaction.AccountOwnerId = entity.Vehicle.MobileUserId.Value;
                accountTransaction.AccountOwnerType = (int)AccountOwnerTypeEnum.MobileUser;
            }
            accountTransaction.Debt = entity.Cost;
            accountTransaction.IsDeleted = false;
            accountTransaction.TransactionType = (int)TransactionTypeEnum.Payable;
            accountTransaction.TransactionDate = DateTime.UtcNow;

            await dbContext.AddAsync(accountTransaction, cancellationToken);
        }

        dbContext.MainServices.Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        var resultDto = entity.FromEntity();

        await cacheService.UpdateToListAsync($"{redisKeyPrefix}:{currentUser.CompanyId!}", request.Id.ToString(), resultDto, null);

        return Response<MainServiceDto>.Success(resultDto);
    }

    private async Task<decimal> GetBalance(Guid customerId, CancellationToken cancellationToken)
    {
        var totalDebt = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == Guid.Parse(currentUser.CompanyId!))
            .SumAsync(i => i.Debt, cancellationToken);

        var totalClaim = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == Guid.Parse(currentUser.CompanyId!))
            .SumAsync(i => i.Claim, cancellationToken);

        return Math.Abs(totalDebt - totalClaim);
    }

    private async Task<decimal> GetTotalPrice(Guid mainServiceId, CancellationToken cancellationToken)
    {
        var totalPrice = await dbContext.SubServices.AsNoTracking()
            .Where(i => i.MainServiceId == mainServiceId)
            .SumAsync(i => i.Cost, cancellationToken);

        var totalDiscount = await dbContext.SubServices.AsNoTracking()
            .Where(i => i.MainServiceId == mainServiceId && i.Discount.HasValue)
            .SumAsync(i => i.Discount, cancellationToken);

        return totalPrice - (totalDiscount ?? 0);
    }
}
