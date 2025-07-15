using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Queries.GetList;

public record GetListMainServiceByMyVehicleQuery(PageRequest PageRequest, Guid VehicleId)
    : IRequest<Response<Paginate<MainServiceDto>>>;

public class GetListMainServiceByMyVehicleHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetListMainServiceByMyVehicleQuery, Response<Paginate<MainServiceDto>>>
{
    public async Task<Response<Paginate<MainServiceDto>>> Handle(GetListMainServiceByMyVehicleQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(currentUser.Id!);
        var query = unitOfWork.MainServices.GetByVehicleIdWithUser(request.VehicleId, userId, true);

            var result = await query
                .OrderByDescending(i => i.ServiceDate)
                .Select(i => i.FromEntity())
                .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<MainServiceDto>>.Success(result);
    }
}

