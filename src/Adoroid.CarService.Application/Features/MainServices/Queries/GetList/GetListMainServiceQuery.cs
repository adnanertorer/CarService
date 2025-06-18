using Adoroid.CarService.Application.Common.Abstractions.Auth;
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

public class GetListMainServiceQueryHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<GetListMainServiceQuery, Response<Paginate<MainServiceDto>>>
{

    public async Task<Response<Paginate<MainServiceDto>>> Handle(GetListMainServiceQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.MainServices
            .Include(i => i.Vehicle)
            .ThenInclude(i => i!.Customer)
            .AsNoTracking()
            .Where(i => i.Vehicle != null && i.Vehicle.Customer != null && i.Vehicle!.Customer!.CompanyId == Guid.Parse(currentUser.CompanyId!));

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
            query = query.Where(i => i.Vehicle!.Brand.Contains(request.FilterRequest.Search) || i.Vehicle!.Model.Contains(request.FilterRequest.Search)
            || i.Vehicle!.Plate.Contains(request.FilterRequest.Search));

        var result = await query
            .OrderByDescending(i => i.ServiceDate)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.FilterRequest.PageRequest.PageIndex, request.FilterRequest.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<MainServiceDto>>.Success(result);
    }
}
