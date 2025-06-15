using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Update;

public record UpdateVehicleCommand(Guid Id, string Brand, string Model, int Year, string Plate, int FuelTypeId, string? Engine, string? SerialNumber) :
    IRequest<Response<VehicleDto>>;

public class UpdateVehicleCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<UpdateVehicleCommand, Response<VehicleDto>{
    public async Task<Response<VehicleDto>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await dbContext.Vehicles
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (vehicle is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.NotFound);

        vehicle.UpdatedDate = DateTime.UtcNow;
        vehicle.UpdatedBy = Guid.Parse(currentUser.Id!);
        vehicle.FuelTypeId = request.FuelTypeId;
        vehicle.SerialNumber = request.SerialNumber;
        vehicle.Engine = request.Engine;
        vehicle.Model = request.Model;
        vehicle.Year = request.Year;
        vehicle.Plate = request.Plate;
        vehicle.Brand = request.Brand;

        dbContext.Vehicles.Update(vehicle);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<VehicleDto>.Success(vehicle.FromEntity());
    }
}
