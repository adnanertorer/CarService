using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Districts.Queries.GetList;

public record GetDistrictListQuery(int CityId, string? Search) 
    : IRequest<Response<Paginate<District>>>;

public class GetDistrictListQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService) :
    IRequestHandler<GetDistrictListQuery, Response<Paginate<District>>>
{
    public async Task<Response<Paginate<District>>> Handle(GetDistrictListQuery request, CancellationToken cancellationToken)
    {
        string redisKeyPrefix = $"district:list:{request.CityId}";

        var list = await cacheService.GetOrSetListAsync<List<District>>(redisKeyPrefix, async() => {
            var list = await unitOfWork.Districts.GetDistricts(request.CityId, cancellationToken);
            return [.. list];
        }
        , TimeSpan.FromHours(7));

        return Response<Paginate<District>>.Success(list.AsQueryable().ToPaginate(0, 100));
    }
}
