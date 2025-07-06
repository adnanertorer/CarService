using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Update;

public record AssignVehicleToUserCommand(Guid VehicleId) 
    : IRequest<Response<VehicleDto>>;

public class AssingVehicleToUserCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ILogger<AssingVehicleToUserCommandHandler> logger)
    : IRequestHandler<AssignVehicleToUserCommand, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(AssignVehicleToUserCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(currentUser.Id!);

        var vehicle = await dbContext.Vehicles
            .Include(i => i.MainServices)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.VehicleId, cancellationToken);

        if (vehicle is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.NotFound);

        //kendisine atanmis gecici kullanicilari buluyoruz
        var vehicleTemporaryUsers = await dbContext.VehicleUsers
            .Where(i => i.VehicleId == request.VehicleId && i.UserTypeId == (int)VehicleUserTypeEnum.Temporary)
            .ToListAsync(cancellationToken);

        if (vehicleTemporaryUsers.Count == 0)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.VehicleUserRecordNotFound);

        var vehicleUser = vehicleTemporaryUsers.First(); // ilk kaydi aliyoruz

        var oldUserId = vehicleUser.UserId; //eski id alınıyor, customer tablo bakılacak.
                                            //Customer tablosundaki customerId ile eşleşen userId ise,
                                            //customer tablosundaki mobileuserId ve bilgiler güncellenecek.

        vehicleUser.UpdatedDate = DateTime.UtcNow;
        vehicleUser.UserId = userId; //mobil kullanicinin id'si atanıyor
        vehicleUser.UserTypeId = (int)VehicleUserTypeEnum.Master;
        vehicleUser.UpdatedBy = userId;
       
        
        foreach (var item in vehicleTemporaryUsers.Where(i => i.UserId != userId))
        {
            item.DeletedBy = userId;
            item.DeletedDate = DateTime.UtcNow;
            item.IsDeleted = true;
        }

        // Eğer eski kullanıcı bir müşteri ise, müşteri tablosundaki userId ve bilgileri güncelle
        var currentCustomer = await dbContext.Customers
            .FirstOrDefaultAsync(i => i.Id == oldUserId, cancellationToken);

        if (currentCustomer is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.CustomerNotFound);

        var mobileUser = await dbContext.MobileUsers
            .FirstOrDefaultAsync(i => i.Id == userId, cancellationToken);

        if (mobileUser is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.MobileUserNotFound);

        currentCustomer.MobileUserId = userId;
        currentCustomer.Name = mobileUser.Name;
        currentCustomer.Surname = mobileUser.Surname;
        currentCustomer.Email = mobileUser.Email;
        currentCustomer.Phone = mobileUser.PhoneNumber;
        currentCustomer.Address = mobileUser.Address;
        currentCustomer.CityId = mobileUser.CityId;
        currentCustomer.DistrictId = mobileUser.DistrictId;
        currentCustomer.UpdatedBy = userId;
        currentCustomer.UpdatedDate = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);


        logger.LogInformation("Vehicle {VehicleId} assigned to user {UserId}", request.VehicleId, userId);

        return Response<VehicleDto>.Success(vehicle.FromEntity());
    }
}

