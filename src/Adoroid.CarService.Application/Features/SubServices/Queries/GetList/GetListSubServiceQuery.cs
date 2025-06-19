using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetList;

public record GetListSubServiceQuery(PageRequest PageRequest, Guid MainServiceId) : IRequest<Response<Paginate<SubServiceDto>>>;

public record GetListSubServiceQueryHandler(PageRequest PageRequest, string? Search)
    : IRequest<Response<Paginate<SubServiceDto>>>;

public class GetEntityListQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetListSubServiceQuery, Response<Paginate<SubServiceDto>>>
{

    public async Task<Response<Paginate<SubServiceDto>>> Handle(GetListSubServiceQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.SubServices
            .Include(i => i.MainService).ThenInclude(i => i.Vehicle)
            .Include(i => i.Employee)
            .Include(i => i.Supplier)
            .AsNoTracking()
            .Where(i => i.MainServiceId == request.MainServiceId);


            var result = await query
                .OrderByDescending(i => i.OperationDate)
                .Select(i => i.FromEntity())
                .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<SubServiceDto>>.Success(result);
    }
}
