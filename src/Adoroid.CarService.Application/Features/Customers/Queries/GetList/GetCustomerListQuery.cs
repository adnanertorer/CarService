using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Application.Features.Customers.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Queries.GetList;

public record GetCustomerListQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<CustomerDto>>>;

public class GetCustomerListQueryHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<GetCustomerListQuery, Response<Paginate<CustomerDto>>>
{
    public async Task<Response<Paginate<CustomerDto>>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Customers
             .AsNoTracking()
             .Include(i => i.Vehicles)
             .Where(i => i.CompanyId == Guid.Parse(currentUser.CompanyId!));

        if (!string.IsNullOrWhiteSpace(request.Search)) 
            query = query.Where(i => i.Name.Contains(request.Search) || i.Surname.Contains(request.Search) || i.Phone.Contains(request.Search));

        var result = await query.OrderBy(i => i.Name)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<CustomerDto>>.Success(result);
    }
}