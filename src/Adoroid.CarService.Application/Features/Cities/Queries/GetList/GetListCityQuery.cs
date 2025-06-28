using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Cities.Queries.GetList;

public record GetListCityQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<City>>>;

public class GetListCityQueryHandler(CarServiceDbContext dbContext, ICacheService cacheService) : IRequestHandler<GetListCityQuery, Response<Paginate<City>>>
{
    const string redisKeyPrefix = "city:list";
    public async Task<Response<Paginate<City>>> Handle(GetListCityQuery request, CancellationToken cancellationToken)
    {

        var list = await cacheService.GetOrSetPaginateAsync<List<City>>(redisKeyPrefix, async () =>
        {
            var query = dbContext.Cities
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(i => i.Name.Contains(request.Search));

            return await query
                  .OrderBy(i => i.Name)
                  .ToListAsync(cancellationToken);
        }, TimeSpan.FromHours(7));

        return Response<Paginate<City>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, 100));
    }
}