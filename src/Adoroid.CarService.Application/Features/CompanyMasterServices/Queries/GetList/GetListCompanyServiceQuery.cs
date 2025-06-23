using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Application.Features.CompanyMasterServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Queries.GetList;

public record GetListCompanyServiceQuery(PageRequest PageRequest, string? Search)
    : IRequest<Response<Paginate<CompanyServiceDto>>>;

public class GetListCompanyServiceQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetListCompanyServiceQuery, Response<Paginate<CompanyServiceDto>>>
{

    public async Task<Response<Paginate<CompanyServiceDto>>> Handle(GetListCompanyServiceQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.CompanyServices
            .Include(i => i.MasterService)
            .Include(i => i.Company)
            .AsNoTracking();


        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(i => i.MasterService.ServiceName.Contains(request.Search));


            var result = await query
                .OrderBy(i => i.MasterService.ServiceName)
                .Select(i => i.FromEntity())
                .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<CompanyServiceDto>>.Success(result);
    }
}

