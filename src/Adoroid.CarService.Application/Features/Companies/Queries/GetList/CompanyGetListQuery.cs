using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Queries.GetList;

public record CompanyGetListQuery(PageRequest PageRequest, int? CityId, int? DistrictId, string? Search) : IRequest<Response<Paginate<CompanyDto>>>;

public class CompanyGetListQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CompanyGetListQuery, Response<Paginate<CompanyDto>>>
{
    public async Task<Response<Paginate<CompanyDto>>> Handle(CompanyGetListQuery request, CancellationToken cancellationToken)
    {
        var companies = unitOfWork.Companies.GetAllWithIncludes();

        if (request.CityId.HasValue)
            companies = companies.Where(i => i.CityId == request.CityId);

        if(request.DistrictId.HasValue)
            companies = companies.Where(i => i.DistrictId == request.DistrictId);


        if(!string.IsNullOrWhiteSpace(request.Search))
           companies =  companies.Where(i => i.AuthorizedName.Contains(request.Search) || i.AuthorizedSurname.Contains(request.Search)
            || i.TaxNumber.Contains(request.Search) || i.TaxOffice.Contains(request.Search) || i.CompanyName.Contains(request.Search)
            || i.CompanyPhone.Contains(request.Search) || i.CompanyEmail.Contains(request.Search)
            || i.CompanyServices.Any(i=>i.MasterService.ServiceName.Contains(request.Search)));

        var result = await companies.OrderBy(i => i.CompanyName)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<CompanyDto>>.Success(result);
    }
}
