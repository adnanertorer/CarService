using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Employees.Dtos;
using Adoroid.CarService.Application.Features.Employees.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Employees.Queries.GetList;

public record GetEmployeeListQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<EmployeeDto>>>;

public class GetEmployeeListQueryHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<GetEmployeeListQuery, Response<Paginate<EmployeeDto>>>
{
    public async Task<Response<Paginate<EmployeeDto>>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Employees
             .AsNoTracking()
             .Where(i => i.CompanyId == Guid.Parse(currentUser.CompanyId!));

        if(!string.IsNullOrEmpty(request.Search))
            query = query.Where(i => i.Name.Contains(request.Search) || i.Surname.Contains(request.Search)
            || i.PhoneNumber.Contains(request.Search));

        var result = await query.OrderBy(i => i.Name)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<EmployeeDto>>.Success(result);
    }
}
