using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetBySerialNumber;

public record GetBySerialNumberQuery(string PlateNumber, string SerialNumber) : IRequest<Response<VehicleDto>>;

public class GetBySerialNumberQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetBySerialNumberQuery, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(GetBySerialNumberQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.Vehicles.GetBySerialNumber(request.PlateNumber, request.SerialNumber, cancellationToken);

        if (entity is null)
            return Response<VehicleDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<VehicleDto>.Success(entity.FromEntity());
    }
}

