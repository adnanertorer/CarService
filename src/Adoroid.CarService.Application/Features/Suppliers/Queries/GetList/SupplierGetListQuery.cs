﻿using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Suppliers.Queries.GetList;

public record SupplierGetListQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<SupplierDto>>>;

public class SupplierGetListQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<SupplierGetListQuery, Response<Paginate<SupplierDto>>>
{
    public async Task<Response<Paginate<SupplierDto>>> Handle(SupplierGetListQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var query = unitOfWork.Suppliers.GetAll(companyId, cancellationToken);

        if(!string.IsNullOrEmpty(request.Search))
            query = query.Where(i => i.ContactName.Contains(request.Search) || i.Name.Contains(request.Search)
            || i.PhoneNumber.Contains(request.Search));

        var result = await query.OrderBy(i => i.Name)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<SupplierDto>>.Success(result);
    }
}
