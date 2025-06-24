using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Enums;
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
    : IRequest<Response<MainServiceDto>>, ICacheRemovableCommand
{
    public IEnumerable<string> GetCacheKeysToRemove()
    {
        yield return $"main-service:{Id}";
        yield return "main-service:list";
    }
}

public class UpdateMainServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
        : IRequestHandler<UpdateMainServiceCommand, Response<MainServiceDto>>
{

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

            var balance = await GetBalance(entity.Vehicle!.CustomerId, cancellationToken);
            var accountTransaction = new AccountingTransaction
            {
                Balance = balance + entity.Cost,
                Claim = 0,
                CompanyId = Guid.Parse(currentUser.CompanyId!),
                CreatedBy = Guid.Parse(currentUser.Id!),
                CreatedDate = DateTime.UtcNow,
                CustomerId = entity.Vehicle!.CustomerId,
                Debt = entity.Cost,
                IsDeleted = false,
                TransactionType = (int)TransactionTypeEnum.Payable,
                TransactionDate = DateTime.UtcNow
            };

            await dbContext.AddAsync(accountTransaction, cancellationToken);
        }

        dbContext.MainServices.Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);


        return Response<MainServiceDto>.Success(entity.FromEntity());
    }

    private async Task<decimal> GetBalance(Guid customerId, CancellationToken cancellationToken)
    {
        var totalDebt = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.CustomerId == customerId && i.CompanyId == Guid.Parse(currentUser.CompanyId!))
            .SumAsync(i => i.Debt, cancellationToken);

        var totalClaim = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.CustomerId == customerId && i.CompanyId == Guid.Parse(currentUser.CompanyId!))
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
