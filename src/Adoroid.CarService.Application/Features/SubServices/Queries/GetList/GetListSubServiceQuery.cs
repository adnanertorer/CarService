using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetList;

public record GetListSubServiceQuery(PageRequest PageRequest, Guid MainServiceId) : IRequest<Response<Paginate<SubServiceDto>>>;

public record GetListSubServiceQueryHandler(PageRequest PageRequest, string? Search)
    : IRequest<Response<Paginate<SubServiceDto>>>;

public class GetEntityListQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService, ICurrentUser currentUser)
    : IRequestHandler<GetListSubServiceQuery, Response<Paginate<SubServiceDto>>>
{
  
    public async Task<Response<Paginate<SubServiceDto>>> Handle(GetListSubServiceQuery request, CancellationToken cancellationToken)
    {
        var redisKeyPrefix = $"subservice:list:{request.MainServiceId}";

        var companyId = currentUser.ValidCompanyId();

        var cacheKey = $"{redisKeyPrefix}:{companyId}";

        var list = await cacheService.GetOrSetPaginateAsync<List<SubServiceDto>>(cacheKey, async () =>
        {
            var query = unitOfWork.SubServices.GetListByMainServiceIdWithDetails(request.MainServiceId, true);

            return await query
                .OrderByDescending(i => i.OperationDate)
                .Select(i => i.FromEntity()).ToListAsync(cancellationToken);
        }, TimeSpan.FromHours(2));

        return Response<Paginate<SubServiceDto>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, request.PageRequest.PageSize));
    }
}
