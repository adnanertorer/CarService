using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.BusinessMessages;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.CarService.Application.Features.Users.Queries.CheckCompanyId;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetList;

public record GetListSubServiceQuery(PageRequest PageRequest, Guid MainServiceId) : IRequest<Response<Paginate<SubServiceDto>>>;

public record GetListSubServiceQueryHandler(PageRequest PageRequest, string? Search)
    : IRequest<Response<Paginate<SubServiceDto>>>;

public class GetEntityListQueryHandler(CarServiceDbContext dbContext, ICacheService cacheService, IMediator mediator)
    : IRequestHandler<GetListSubServiceQuery, Response<Paginate<SubServiceDto>>>
{
    const string redisKeyPrefix = "subservice:list";
    public async Task<Response<Paginate<SubServiceDto>>> Handle(GetListSubServiceQuery request, CancellationToken cancellationToken)
    {
        var companyIdResponse = await mediator.Send(new GetCompanyIdCommand(), cancellationToken);

        if (!companyIdResponse.Succeeded)
            return Response<Paginate<SubServiceDto>>.Fail(BusinessMessages.CompanyNotFound);

        var companyId = companyIdResponse.Data!.Value;

        var cacheKey = $"{redisKeyPrefix}:{companyId}";

        var list = await cacheService.GetOrSetPaginateAsync<List<SubServiceDto>>(cacheKey, async () =>
        {
            var query = dbContext.SubServices
               .Include(i => i.MainService).ThenInclude(i => i.Vehicle)
               .Include(i => i.Employee)
               .Include(i => i.Supplier)
               .AsNoTracking()
               .Where(i => i.MainServiceId == request.MainServiceId);

            return await query
                .OrderByDescending(i => i.OperationDate)
                .Select(i => i.FromEntity()).ToListAsync(cancellationToken);
        }, TimeSpan.FromHours(2));

        return Response<Paginate<SubServiceDto>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, request.PageRequest.PageSize));
    }
}
