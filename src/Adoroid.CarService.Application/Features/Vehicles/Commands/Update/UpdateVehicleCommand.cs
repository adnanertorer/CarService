using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Update;

public record UpdateVehicleCommand(Guid Id, string Brand, string Model, int Year, string Plate, int FuelTypeId, string? Engine, string? SerialNumber) :
    IRequest<Response<VehicleDto>>;

public class UpdateVehicleCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdateVehicleCommand, Response<VehicleDto>>{
    public async Task<Response<VehicleDto>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await unitOfWork.Vehicles.GetByIdAsync(request.Id, false, cancellationToken);

        if (vehicle is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.NotFound);

        var isVehicleNotTemporary = await unitOfWork.VehicleUsers.IsVehicleNotTempoary(request.Id, cancellationToken);

        if (isVehicleNotTemporary && currentUser.UserType == "company")
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.VehicleIsNotTemporary);

        vehicle.UpdatedDate = DateTime.UtcNow;
        vehicle.UpdatedBy = Guid.Parse(currentUser.Id!);
        vehicle.FuelTypeId = request.FuelTypeId;
        vehicle.SerialNumber = request.SerialNumber;
        vehicle.Engine = request.Engine;
        vehicle.Model = request.Model;
        vehicle.Year = request.Year;
        vehicle.Plate = request.Plate;
        vehicle.Brand = request.Brand;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<VehicleDto>.Success(vehicle.FromEntity());
    }
}
