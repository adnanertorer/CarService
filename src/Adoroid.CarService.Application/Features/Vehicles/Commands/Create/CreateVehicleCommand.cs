using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Create;

public record CreateVehicleCommand(Guid? CustomerId, string Brand, string Model, int Year, string Plate,
    int FuelTypeId, string? Engine, string SerialNumber) : IRequest<Response<VehicleDto>>;

public class CreateVehicleCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<CreateVehicleCommand, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var isExist = await unitOfWork.Vehicles.ExistsAsync(request.Plate, request.SerialNumber, cancellationToken); ;

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

        var resultEntity = await unitOfWork.Vehicles.AddAsync(entity, cancellationToken);

        if(request.CustomerId.HasValue)
        {
            var vehicleUser = new VehicleUser
            {
                VehicleId = resultEntity.Id,
                UserId = request.CustomerId.Value,
                UserTypeId = (int)VehicleUserTypeEnum.Temporary, // Geçici kullanıcı olarak işaretleniyor
                CreatedDate = DateTime.UtcNow,
                CreatedBy = Guid.Parse(currentUser.Id!)
            };
            await unitOfWork.VehicleUsers.AddAsync(vehicleUser, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<VehicleDto>.Success(resultEntity.FromEntity());
    }
}