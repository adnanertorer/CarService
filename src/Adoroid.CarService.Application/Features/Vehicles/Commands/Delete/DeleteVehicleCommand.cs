using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;

namespace Adoroid.CarService.Application.Features.Vehicles.Commands.Delete;

public record DeleteVehicleCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteVehicleCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<DeleteVehicleCommand, Response<Guid>{
    public async Task<Response<Guid>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await dbContext.Vehicles
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (vehicle is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        vehicle.DeletedDate = DateTime.UtcNow;
        vehicle.DeletedBy = Guid.Parse(currentUser.Id!);
        vehicle.IsDeleted = true;

        dbContext.Vehicles.Update(vehicle);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(request.Id);
    }
}
