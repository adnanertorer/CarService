using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Delete;

public record DeleteVehicleCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteVehicleCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<DeleteVehicleCommand, Response<Guid>>{
    public async Task<Response<Guid>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await unitOfWork.Vehicles.GetByIdWithMainServiceAsync(request.Id, false, cancellationToken);

        var isVehicleNotTemporary = await unitOfWork.VehicleUsers.IsVehicleNotTempoary(request.Id, cancellationToken);

        if(isVehicleNotTemporary) 
            return Response<Guid>.Fail(BusinessExceptionMessages.VehicleIsNotTemporary);

        if (vehicle is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        if(vehicle.MainServices != null && vehicle.MainServices.Count != 0)
            return Response<Guid>.Fail(BusinessExceptionMessages.VehicleHasMainServices);

        vehicle.DeletedDate = DateTime.UtcNow;
        vehicle.DeletedBy = Guid.Parse(currentUser.Id!);
        vehicle.IsDeleted = true;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(request.Id);
    }
}
