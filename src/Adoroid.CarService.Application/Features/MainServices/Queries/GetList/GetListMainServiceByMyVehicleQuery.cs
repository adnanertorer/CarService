using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adoroid.CarService.Application.Features.MainServices.Queries.GetList;

public record GetListMainServiceByMyVehicleQuery(PageRequest PageRequest, Guid VehicleId)
    : IRequest<Response<Paginate<MainServiceDto>>>;

public class GetListMainServiceByMyVehicleHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<GetListMainServiceByMyVehicleQuery, Response<Paginate<MainServiceDto>>>
{
    public async Task<Response<Paginate<MainServiceDto>>> Handle(GetListMainServiceByMyVehicleQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(currentUser.Id!);
        var query = dbContext.MainServices
            .Include(i => i.Vehicle).ThenInclude(v => v!.VehicleUsers)
            .AsNoTracking()
            .Where(i => i.VehicleId == request.VehicleId && i.Vehicle!.VehicleUsers.Any(vu => vu.UserId == userId));

            var result = await query
                .OrderByDescending(i => i.ServiceDate)
                .Select(i => i.FromEntity())
                .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<MainServiceDto>>.Success(result);
    }
}

