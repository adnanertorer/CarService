using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Common.Enums;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Create;

public record CreateVehicleCommand(Guid? CustomerId, string Brand, string Model, int Year, string Plate,
    int FuelTypeId, string? Engine, string SerialNumber) : IRequest<Response<VehicleDto>>;

public class CreateVehicleCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<CreateVehicleCommand, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var isExist = await dbContext.Vehicles
            .AsNoTracking()
            .AnyAsync(i => i.Plate == request.Plate && i.SerialNumber == request.SerialNumber, cancellationToken);

        if (isExist)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.AlreadyExists);

        var entity = new Vehicle
        {
            Brand = request.Brand,
            CreatedBy = Guid.Parse(currentUser.Id!),
            Engine = request.Engine,
            FuelTypeId = request.FuelTypeId,
            IsDeleted = false,
            Model = request.Model,
            Plate = request.Plate,
            SerialNumber = request.SerialNumber,
            Year = request.Year,
            CreatedDate = DateTime.UtcNow
        };

        var resultEntity = await dbContext.Vehicles.AddAsync(entity, cancellationToken);

        if(request.CustomerId.HasValue)
        {
            var vehicleUser = new VehicleUser
            {
                VehicleId = resultEntity.Entity.Id,
                UserId = request.CustomerId.Value,
                UserTypeId = (int)VehicleUserTypeEnum.Temporary, // Geçici kullanıcı olarak işaretleniyor
                CreatedDate = DateTime.UtcNow,
                CreatedBy = Guid.Parse(currentUser.Id!)
            };
            await dbContext.VehiclUsers.AddAsync(vehicleUser, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<VehicleDto>.Success(resultEntity.Entity.FromEntity());
    }
}