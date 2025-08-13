using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Update;

public record UpdateMainServiceCommand(Guid Id, Guid VehicleId, DateTime ServiceDate, string? Description, int MainServiceStatus)
    : IRequest<Response<MainServiceDto>>;

public class UpdateMainServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService,
    ILogger<UpdateMainServiceCommandHandler> logger)
        : IRequestHandler<UpdateMainServiceCommand, Response<MainServiceDto>>
{
    const string redisKeyPrefix = "mainservice:list";
    public async Task<Response<MainServiceDto>> Handle(UpdateMainServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var userId = Guid.Parse(currentUser.Id!);

        var entity = await unitOfWork.MainServices.GetByIdWithVehiclesAsync(request.Id, false, cancellationToken);

        if (entity is null)
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        entity.ServiceDate = request.ServiceDate;
        entity.Description = request.Description;
        entity.VehicleId = request.VehicleId;
        entity.ServiceStatus = request.MainServiceStatus;

        entity.UpdatedBy = userId;
        entity.UpdatedDate = DateTime.UtcNow;

        if(request.MainServiceStatus == (int)MainServiceStatusEnum.Done)
        {
            entity.Cost = await unitOfWork.SubServices.GetTotalPrice(request.Id, cancellationToken);
            entity.MaterialCost = await unitOfWork.SubServices.GetTotalMaterialCost(request.Id, cancellationToken);

            if (entity.Vehicle is null)
                return Response<MainServiceDto>.Fail(BusinessExceptionMessages.VehicleNotFound);

            decimal balance = 0;
            Guid? vehicleUserId = null;
            bool temporaryUser = false;

            if (entity.Vehicle.VehicleUsers is null || entity.Vehicle.VehicleUsers.Count == 0)
                return Response<MainServiceDto>.Fail(BusinessExceptionMessages.VehicleUserNotFound);

            vehicleUserId = entity.Vehicle.VehicleUsers!.FirstOrDefault(i => i.UserTypeId == (int)VehicleUserTypeEnum.Master)?.UserId;
            if(vehicleUserId is null)
            {
                vehicleUserId = entity.Vehicle.VehicleUsers!.FirstOrDefault(i => i.UserTypeId == (int)VehicleUserTypeEnum.Temporary)?.UserId;
                if(vehicleUserId is null)
                    return Response<MainServiceDto>.Fail(BusinessExceptionMessages.VehicleUserNotFound);
                temporaryUser = true;
            }

            balance = await unitOfWork.AccountTransactions.GetBalanceAsync(vehicleUserId!.Value, companyId, cancellationToken);

            var accountTransaction = new AccountingTransaction();
            accountTransaction.Balance = balance + entity.Cost;
            accountTransaction.Claim = 0;
            accountTransaction.CompanyId = companyId;
            accountTransaction.CreatedBy = userId;
            accountTransaction.CreatedDate = DateTime.UtcNow;
            accountTransaction.AccountOwnerId = vehicleUserId.Value;

            if (!temporaryUser)
            {
                accountTransaction.AccountOwnerType = (int)AccountOwnerTypeEnum.MobileUser;
            }
            else
            {
                accountTransaction.AccountOwnerType = (int)AccountOwnerTypeEnum.Customer;
            }
            accountTransaction.Debt = entity.Cost;
            accountTransaction.IsDeleted = false;
            accountTransaction.TransactionType = (int)TransactionTypeEnum.Payable;
            accountTransaction.TransactionDate = DateTime.UtcNow;
            accountTransaction.Description = $"Servis ücreti: {entity.Cost} TL. Araç: {entity.Vehicle.Plate} - {entity.Vehicle.Model} - {entity.Vehicle.Brand}";
            accountTransaction.MainServiceId = request.Id;

            await unitOfWork.AccountTransactions.AddAsync(accountTransaction, cancellationToken);
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken); ;

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            const string errorMessage = "MainService güncelleme işlemi başarısız oldu. MainServiceId: {MainServiceId}";
            logger.LogError(ex, errorMessage, request.Id);
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.MainServiceUpdateError);
        }

        var resultDto = entity.FromEntity();

        try
        {
            await cacheService.UpdateToListAsync($"{redisKeyPrefix}:{companyId}", request.Id.ToString(), resultDto, null);
        }
        catch (Exception ex) {
            const string errorMessage = "Cache güncelleme işlemi başarısız oldu. MainServiceId: {MainServiceId}";
            logger.LogError(ex, errorMessage, request.Id);
        }
       
        return Response<MainServiceDto>.Success(resultDto);
    }
}
