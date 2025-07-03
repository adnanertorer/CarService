using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Dtos.Filters;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Queries.GetList;


public record GetListMainServiceQuery(MainFilterRequestModel FilterRequest)
    : IRequest<Response<Paginate<MainServiceDto>>>;

public class GetListMainServiceQueryHandler(CarServiceDbContext dbContext, ICacheService cacheService, ICurrentUser currentUser)
    : IRequestHandler<GetListMainServiceQuery, Response<Paginate<MainServiceDto>>>
{
    const string redisKeyPrefix = "mainservice:list";
    public async Task<Response<Paginate<MainServiceDto>>> Handle(GetListMainServiceQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var cacheKey = $"{redisKeyPrefix}:{companyId}";

        var list = await cacheService.GetOrSetPaginateAsync<List<MainServiceDto>>(cacheKey,
            async () =>
            {
                var query = dbContext.MainServices
                    .Include(i => i.Vehicle).ThenInclude(i => i.VehicleUsers)
                    .AsNoTracking()
                    .Where(i => i.Vehicle != null && i.CompanyId == companyId);

                if (request.FilterRequest.StartDate.HasValue && request.FilterRequest.EndDate.HasValue)
                {
                    query = query.WhereTwoDateIsBetween(
                        i => i.ServiceDate,
                        request.FilterRequest.StartDate.Value.Date,
                        request.FilterRequest.EndDate.Value.Date.AddDays(1));
                }
                else if (request.FilterRequest.StartDate.HasValue)
                {
                    query = query.WhereDateIsBetween(i => i.ServiceDate, request.FilterRequest.StartDate.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.FilterRequest.Search))
                {
                    var search = request.FilterRequest.Search;
                    query = query.Where(i =>
                        i.Vehicle!.Brand.Contains(search) ||
                        i.Vehicle!.Model.Contains(search) ||
                        i.Vehicle!.Plate.Contains(search));
                }

                return await query
                    .OrderByDescending(i => i.ServiceDate)
                    .Select(i => i.FromEntity()).ToListAsync(cancellationToken);
            }, TimeSpan.FromHours(2));

        return Response<Paginate<MainServiceDto>>.Success(list.AsQueryable().ToPaginate(request.FilterRequest.PageRequest.PageIndex, request.FilterRequest.PageRequest.PageSize));
    }
}
