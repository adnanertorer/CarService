using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MasterServices.Dtos;
using Adoroid.CarService.Application.Features.MasterServices.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MasterServices.Queries.GetList;

public record GetListMasterServicesQuery(PageRequest PageRequest, string? Search)
    : IRequest<Response<Paginate<MasterServiceDto>>>;

public class GetListMasterServicesQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
    : IRequestHandler<GetListMasterServicesQuery, Response<Paginate<MasterServiceDto>>>
{
    const string redisKeyPrefix = "mainservice:list";
    public async Task<Response<Paginate<MasterServiceDto>>> Handle(GetListMasterServicesQuery request, CancellationToken cancellationToken)
    {
        var list = await cacheService.GetOrSetListAsync<List<MasterServiceDto>>(
            redisKeyPrefix,
            async () =>
            {
                var query = unitOfWork.MasterServices.GetQueryable();

                if (!string.IsNullOrEmpty(request.Search))
                    query = query.Where(i => i.ServiceName.Contains(request.Search));

                return await query
                    .OrderBy(i => i.OrderIndex)
                    .Select(i => i.FromEntity())
                    .ToListAsync(cancellationToken);
            },
            TimeSpan.FromHours(2));
      

        return Response<Paginate<MasterServiceDto>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, request.PageRequest.PageSize));
    }
}

