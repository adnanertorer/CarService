using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Districts.Queries.GetList;

public record GetDistrictListQuery(int CityId, string? Search) 
    : IRequest<Response<Paginate<District>>>;

public class GetDistrictListQueryHandler(CarServiceDbContext dbContext, ICacheService cacheService) :
    IRequestHandler<GetDistrictListQuery, Response<Paginate<District>>>
{
    public async Task<Response<Paginate<District>>> Handle(GetDistrictListQuery request, CancellationToken cancellationToken)
    {
        string redisKeyPrefix = $"district:list:{request.CityId}";

        var list = await cacheService.GetOrSetPaginateAsync<List<District>>(redisKeyPrefix, async() => {
            var query = dbContext.Districts
            .AsNoTracking()
            .Where(i => i.CityId == request.CityId);

            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(i => i.Name.Contains(request.Search));

            return await query
                  .OrderBy(i => i.Name)
                  .ToListAsync(cancellationToken);
        }
        , TimeSpan.FromHours(7));

        return Response<Paginate<District>>.Success(list.AsQueryable().ToPaginate(0, 100));
    }
}
