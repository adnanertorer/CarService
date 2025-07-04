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

public record AssingVehicleToUserCommand(Guid VehicleId) 
    : IRequest<Response<VehicleDto>>;

public class AssingVehicleToUserCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ILogger<AssingVehicleToUserCommandHandler> logger)
    : IRequestHandler<AssingVehicleToUserCommand, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(AssingVehicleToUserCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(currentUser.Id!);

        var vehicle = await dbContext.Vehicles
            .Include(i => i.MainServices)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.VehicleId, cancellationToken);

        if (vehicle is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.NotFound);

        var vehicleTemporaryUsers = await dbContext.VehiclUsers
            .Where(i => i.VehicleId == request.VehicleId && i.UserTypeId == (int)VehicleUserTypeEnum.Temporary)
            .ToListAsync(cancellationToken);

        if (vehicleTemporaryUsers.Count == 0)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.VehicleUserRecordNotFound);

        var vehicleUser = vehicleTemporaryUsers.First();

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

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogWarning("Vehicle {VehicleId} assigned to user {UserId}", request.VehicleId, userId);

        return Response<VehicleDto>.Success(vehicle.FromEntity());
    }
}

