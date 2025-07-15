using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Cities.Queries.GetList;

public record GetListCityQuery() : IRequest<Response<Paginate<City>>>;

public class GetListCityQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService) : IRequestHandler<GetListCityQuery, Response<Paginate<City>>>
{
    const string redisKeyPrefix = "city:list";
    public async Task<Response<Paginate<City>>> Handle(GetListCityQuery request, CancellationToken cancellationToken)
    {

        var list = await cacheService.GetOrSetPaginateAsync<List<City>>(redisKeyPrefix, async () =>
        {
            var list = await unitOfWork.Cities.GetAllAsync(cancellationToken);
            return [.. list];

        }, TimeSpan.FromHours(7));

        return Response<Paginate<City>>.Success(list.AsQueryable().ToPaginate(0, 100));
    }
}