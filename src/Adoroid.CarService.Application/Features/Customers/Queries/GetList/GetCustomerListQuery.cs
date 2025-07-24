using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Queries.GetList;

public record GetCustomerListQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<CustomerDto>>>;

public class GetCustomerListQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetCustomerListQuery, Response<Paginate<CustomerDto>>>
{
    public async Task<Response<Paginate<CustomerDto>>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var query = unitOfWork.Customers.GetAllWithIncludes(companyId);

        if (!string.IsNullOrWhiteSpace(request.Search)) 
            query = query.Where(i => i.Name.Contains(request.Search) || i.Surname.Contains(request.Search) || i.Phone.Contains(request.Search));

        var result = await query
            .Select(g => new CustomerDto
            {
                Address = g.Address,
                CompanyId = g.CompanyId,
                Email = g.Email,
                Id = g.Id,
                IsActive = g.IsActive,
                Name = g.Name,
                Phone = g.Phone,
                Surname = g.Surname,
                TaxNumber = g.TaxNumber,
                TaxOffice = g.TaxOffice,
                DistrictId = g.DistrictId,
                CityId = g.CityId,
                City = g.City != null ? new CityModel
                {
                    Id = g.City.Id,
                    Name = g.City.Name
                } : null,
                District = g.District != null ? new DistrictModel
                {
                    Id = g.District.Id,
                    Name = g.District.Name
                } : null
            })
            .OrderBy(x => x.Name)
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<CustomerDto>>.Success(result);
    }
}