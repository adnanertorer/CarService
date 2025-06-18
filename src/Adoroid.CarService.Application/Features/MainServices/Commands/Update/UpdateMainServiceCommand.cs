using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Update;

public record UpdateMainServiceCommand(Guid Id, Guid VehicleId, DateTime ServiceDate, string? Description) : IRequest<Response<MainServiceDto>>;

public class UpdateMainServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
        : IRequestHandler<UpdateMainServiceCommand, Response<MainServiceDto>>
{

    public async Task<Response<MainServiceDto>> Handle(UpdateMainServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.MainServices.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        entity.ServiceDate = request.ServiceDate;
        entity.Description = request.Description;
        entity.VehicleId = request.VehicleId;

        entity.UpdatedBy = Guid.Parse(currentUser.Id!);
        entity.UpdatedDate = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<MainServiceDto>.Success(entity.FromEntity());
    }
}
