using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetList;

public record GetMyVehicleListQuery(PageRequest PageRequest, string? Search)
    : IRequest<Response<Paginate<VehicleDto>>>;

public class GetMyVehicleListQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetMyVehicleListQuery, Response<Paginate<VehicleDto>>>
{
    public async Task<Response<Paginate<VehicleDto>>> Handle(GetMyVehicleListQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.Vehicles.GetVehiclesByUserIdAsync(Guid.Parse(currentUser.Id!));

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(i => i.Brand.Contains(request.Search) || i.Model.Contains(request.Search) || i.Plate.Contains(request.Search)
            || i.SerialNumber != null && i.SerialNumber.Contains(request.Search));

        var result = await query.OrderBy(i => i.Brand)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<VehicleDto>>.Success(result);
    }
}

