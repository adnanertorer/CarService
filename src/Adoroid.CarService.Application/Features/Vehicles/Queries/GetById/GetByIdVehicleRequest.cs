using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetById;

public record GetByIdVehicleRequest(Guid Id) : IRequest<Response<VehicleDto>>;

public class GetByIdVehicleRequestHandler(CarServiceDbContext dbContext) : IRequestHandler<GetByIdVehicleRequest, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(GetByIdVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await dbContext.Vehicles
           .AsNoTracking()
           .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (vehicle is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<VehicleDto>.Success(vehicle.FromEntity());
    }
}