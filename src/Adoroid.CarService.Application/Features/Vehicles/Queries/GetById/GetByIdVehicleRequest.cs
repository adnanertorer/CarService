using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetById;

public record GetByIdVehicleRequest(Guid Id) : IRequest<Response<VehicleDto>>;

public class GetByIdVehicleRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetByIdVehicleRequest, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(GetByIdVehicleRequest request, CancellationToken cancellationToken)
    {
        var vehicle = await unitOfWork.Vehicles.GetByIdWithVehicleUsersAsync(request.Id, true, cancellationToken);

        if (vehicle is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<VehicleDto>.Success(vehicle.FromEntity());
    }
}