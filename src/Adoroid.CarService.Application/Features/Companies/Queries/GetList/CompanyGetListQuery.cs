using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Queries.GetList;

public record CompanyGetListQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<CompanyDto>>>;

public class CompanyGetListQueryHandler(CarServiceDbContext dbContext) : IRequestHandler<CompanyGetListQuery, Response<Paginate<CompanyDto>>>
{
    public async Task<Response<Paginate<CompanyDto>>> Handle(CompanyGetListQuery request, CancellationToken cancellationToken)
    {
        var companies = dbContext.Companies
            .Include(i => i.City)
            .Include(i => i.District)
            .Include(i =>i.CompanyServices).ThenInclude(i => i.MasterService)
            .AsNoTracking();

        if(!string.IsNullOrWhiteSpace(request.Search))
           companies =  companies.Where(i => i.AuthorizedName.Contains(request.Search) || i.AuthorizedSurname.Contains(request.Search)
            || i.TaxNumber.Contains(request.Search) || i.TaxOffice.Contains(request.Search) || i.CompanyName.Contains(request.Search)
            || i.CompanyPhone.Contains(request.Search) || i.CompanyEmail.Contains(request.Search));

        var result = await companies.OrderBy(i => i.CompanyName)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<CompanyDto>>.Success(result);
    }
}
