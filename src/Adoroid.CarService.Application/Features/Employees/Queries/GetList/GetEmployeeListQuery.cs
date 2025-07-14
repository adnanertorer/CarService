using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Employees.Dtos;
using Adoroid.CarService.Application.Features.Employees.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Employees.Queries.GetList;

public record GetEmployeeListQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<EmployeeDto>>>;

public class GetEmployeeListQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetEmployeeListQuery, Response<Paginate<EmployeeDto>>>
{
    public async Task<Response<Paginate<EmployeeDto>>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var query = unitOfWork.Employees.GetAll(companyId, true, cancellationToken);

        if(!string.IsNullOrEmpty(request.Search))
            query = query.Where(i => i.Name.Contains(request.Search) || i.Surname.Contains(request.Search)
            || i.PhoneNumber.Contains(request.Search));

        var result = await query.OrderBy(i => i.Name)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<EmployeeDto>>.Success(result);
    }
}
