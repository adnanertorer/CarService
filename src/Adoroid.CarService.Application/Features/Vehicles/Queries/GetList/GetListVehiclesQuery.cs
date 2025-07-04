﻿using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetList;

public record GetListVehiclesQuery(PageRequest PageRequest, Guid CustomerId, string? Search) : IRequest<Response<Paginate<VehicleDto>>>;

public class GetListVehiclesQueryHandler(CarServiceDbContext dbContext) : IRequestHandler<GetListVehiclesQuery, Response<Paginate<VehicleDto>>>
{
    public async Task<Response<Paginate<VehicleDto>>> Handle(GetListVehiclesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Vehicles
            .Where(i => i.VehicleUsers.Any(i => i.UserId == request.CustomerId))
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(i => i.Brand.Contains(request.Search) || i.Model.Contains(request.Search) || i.Plate.Contains(request.Search)
            || (i.SerialNumber != null && i.SerialNumber.Contains(request.Search)));

        var result = await query.OrderBy(i => i.Brand)
            .Select(i => i.FromEntity())
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<VehicleDto>>.Success(result);
    }
}