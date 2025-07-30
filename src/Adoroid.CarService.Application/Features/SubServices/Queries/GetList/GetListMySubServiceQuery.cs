using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetList;

public record GetListMySubServiceQuery(PageRequest PageRequest, Guid MainServiceId) : IRequest<Response<Paginate<MobileSubServiceDto>>>;

public class GetListMySubServiceQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService)
    : IRequestHandler<GetListMySubServiceQuery, Response<Paginate<MobileSubServiceDto>>>
{
    const string redisKeyPrefix = "subservice:list";
    private const string MobileUser = "mobileUser";
    public async Task<Response<Paginate<MobileSubServiceDto>>> Handle(GetListMySubServiceQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{redisKeyPrefix}:{currentUser.Id!}";

        if(currentUser.UserType != MobileUser)
            return Response<Paginate<MobileSubServiceDto>>.Fail(BusinessExceptionMessages.YouAreNotAuthorized);

        var list = await cacheService.GetOrSetListAsync<List<MobileSubServiceDto>>(cacheKey, async () =>
        {
            var query = unitOfWork.SubServices.GetByMainServiceIdAndUser(request.MainServiceId, Guid.Parse(currentUser.Id!), true);

            return await query
                .OrderByDescending(i => i.OperationDate)
                .Select(i => i.FromEntityToMobile()).ToListAsync(cancellationToken);
        }, TimeSpan.FromHours(2));

        return Response<Paginate<MobileSubServiceDto>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, request.PageRequest.PageSize));
    }
}

