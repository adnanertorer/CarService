using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.BusinessMessages;
using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;
using Adoroid.CarService.Application.Features.Users.Queries.CheckCompanyId;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.Application.Features.Suppliers.Queries.GetList;

public record SupplierGetListQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<SupplierDto>>>;

public class SupplierGetListQueryHandler(CarServiceDbContext dbContext, IMediator mediator) : IRequestHandler<SupplierGetListQuery, Response<Paginate<SupplierDto>>>
{
    public async Task<Response<Paginate<SupplierDto>>> Handle(SupplierGetListQuery request, CancellationToken cancellationToken)
    {
        var companyIdResponse = await mediator.Send(new GetCompanyIdCommand(), cancellationToken);

        if (!companyIdResponse.Succeeded)
            return Response<Paginate<SupplierDto>>.Fail(BusinessMessages.CompanyNotFound);

        var companyId = companyIdResponse.Data!.Value;

        var query = dbContext.Suppliers.AsNoTracking()
            .Where(i => i.CompanyId == companyId);

        if(!string.IsNullOrEmpty(request.Search))
            query = query.Where(i => i.ContactName.Contains(request.Search) || i.Name.Contains(request.Search)
            || i.PhoneNumber.Contains(request.Search));

        var result = await query.OrderBy(i => i.Name)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<SupplierDto>>.Success(result);
    }
}
