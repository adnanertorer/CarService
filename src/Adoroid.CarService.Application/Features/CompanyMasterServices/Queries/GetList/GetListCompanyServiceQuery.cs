using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Application.Features.CompanyMasterServices.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Queries.GetList;

public record GetListCompanyServiceQuery(PageRequest PageRequest, string? Search)
    : IRequest<Response<Paginate<CompanyServiceDto>>>;

public class GetListCompanyServiceQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetListCompanyServiceQuery, Response<Paginate<CompanyServiceDto>>>
{

    public async Task<Response<Paginate<CompanyServiceDto>>> Handle(GetListCompanyServiceQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.CompanyServices.GetMasterServices(cancellationToken);


        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(i => i.MasterService.ServiceName.Contains(request.Search));


            var result = await query
                .OrderBy(i => i.MasterService.ServiceName)
                .Select(i => i.FromEntity())
                .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<CompanyServiceDto>>.Success(result);
    }
}

