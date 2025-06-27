using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetList;

public record GetListMySubServiceQuery(PageRequest PageRequest, Guid MainServiceId) : IRequest<Response<Paginate<MobileSubServiceDto>>>;

public class GetListMySubServiceQueryHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ICacheService cacheService)
    : IRequestHandler<GetListMySubServiceQuery, Response<Paginate<MobileSubServiceDto>>>
{
    const string redisKeyPrefix = "subservice:list";
    public async Task<Response<Paginate<MobileSubServiceDto>>> Handle(GetListMySubServiceQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{redisKeyPrefix}:{currentUser.Id!}";

        if(currentUser.UserType != "mobileUser")
            return Response<Paginate<MobileSubServiceDto>>.Fail(BusinessExceptionMessages.YouAreNotAuthorized);

        var list = await cacheService.GetOrSetPaginateAsync<List<MobileSubServiceDto>>(cacheKey, async () =>
        {
            var query = dbContext.SubServices
               .Include(i => i.MainService).ThenInclude(i => i.Vehicle)
               .AsNoTracking()
               .Where(i => i.MainServiceId == request.MainServiceId && i.MainService.Vehicle!.MobileUserId == Guid.Parse(currentUser.Id!));

            return await query
                .OrderByDescending(i => i.OperationDate)
                .Select(i => i.FromEntityToMobile()).ToListAsync(cancellationToken);
        }, TimeSpan.FromHours(2));

        return Response<Paginate<MobileSubServiceDto>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, request.PageRequest.PageSize));
    }
}

