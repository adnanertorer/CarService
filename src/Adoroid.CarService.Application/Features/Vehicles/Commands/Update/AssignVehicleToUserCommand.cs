using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
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

        var vehicleTemporaryUsers = await dbContext.VehicleUsers
            .Where(i => i.VehicleId == request.VehicleId && i.UserTypeId == (int)VehicleUserTypeEnum.Temporary)
            .ToListAsync(cancellationToken);

        if (vehicleTemporaryUsers.Count == 0)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.VehicleUserRecordNotFound);

        var vehicleUser = vehicleTemporaryUsers.First();

        var oldUserId = vehicleUser.UserId; //eski id alınıyor, customer tablo bakılacak. Customer tablosundaki customerId ile eşleşen userId ise, customer tablosundaki userId ve bilgiler güncellenecek.

        vehicleUser.UpdatedDate = DateTime.UtcNow;
        vehicleUser.UserId = userId;
        vehicleUser.UserTypeId = (int)VehicleUserTypeEnum.Master;
        vehicleUser.UpdatedBy = userId;
       
        
        foreach (var item in vehicleTemporaryUsers.Where(i => i.UserId != userId))
        {
            item.DeletedBy = userId;
            item.DeletedDate = DateTime.UtcNow;
            item.IsDeleted = true;
        }

        // Eğer eski kullanıcı bir müşteri ise, müşteri tablosundaki userId ve bilgileri güncelle
        var oldCustomer = await dbContext.Customers
            .FirstOrDefaultAsync(i => i.Id == oldUserId, cancellationToken);

        if (oldCustomer is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.CustomerNotFound);

        var mobileUser = await dbContext.MobileUsers
            .FirstOrDefaultAsync(i => i.Id == userId, cancellationToken);

        if (mobileUser is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.MobileUserNotFound);

        oldCustomer.DeletedBy = userId;
        oldCustomer.DeletedDate = DateTime.UtcNow;
        oldCustomer.IsDeleted = true;

        var customer = new Customer
        {
            MobileUserId = userId,
            Name = mobileUser.Name,
            Surname = mobileUser.Surname,
            Email = mobileUser.Email,
            Phone = mobileUser.PhoneNumber,
            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow,
            Address = mobileUser.Address,
            CityId = mobileUser.CityId,
            DistrictId = mobileUser.DistrictId,
            CompanyId = oldCustomer.CompanyId,
            TaxNumber = oldCustomer.TaxNumber,
            IsActive = true,
            TaxOffice = oldCustomer.TaxOffice
        };

        await dbContext.Customers.AddAsync(customer, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Vehicle {VehicleId} assigned to user {UserId}", request.VehicleId, userId);

        return Response<VehicleDto>.Success(vehicle.FromEntity());
    }
}

