using Adoroid.CarService.Application.Features.MasterServices.Dtos;
using Adoroid.CarService.Application.Features.MasterServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MasterServices.Queries.GetList;

public record GetListMasterServicesQuery(PageRequest PageRequest, string? Search)
    : IRequest<Response<Paginate<MasterServiceDto>>>;

public class GetListMasterServicesQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetListMasterServicesQuery, Response<Paginate<MasterServiceDto>>>
{

    public async Task<Response<Paginate<MasterServiceDto>>> Handle(GetListMasterServicesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.MasterServices
            .AsNoTracking();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(i => i.ServiceName.Contains(request.Search));

            var result = await query
                .OrderBy(i => i.OrderIndex)
                .Select(i => i.FromEntity())
                .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<MasterServiceDto>>.Success(result);
    }
}

