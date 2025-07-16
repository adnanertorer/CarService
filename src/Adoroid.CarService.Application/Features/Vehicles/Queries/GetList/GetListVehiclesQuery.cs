using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Vehicles.Queries.GetList;

public record GetListVehiclesQuery(PageRequest PageRequest, Guid CustomerId, string? Search) : IRequest<Response<Paginate<VehicleDto>>>;

public class GetListVehiclesQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetListVehiclesQuery, Response<Paginate<VehicleDto>>>
{
    public async Task<Response<Paginate<VehicleDto>>> Handle(GetListVehiclesQuery request, CancellationToken cancellationToken)
    {
        var customers = unitOfWork.Customers.GetByCompanyId(Guid.Parse(currentUser.CompanyId!));
        var vehicles = unitOfWork.Vehicles.GetQueryable();
        var vehicleUsers = unitOfWork.VehicleUsers.GetQueryable();

        var query = from cus in customers from vu in vehicleUsers
                    where cus.CompanyId == Guid.Parse(currentUser.CompanyId!) 
                    && cus.Id == request.CustomerId 
                    && (cus.Id == vu.UserId || cus.MobileUserId == vu.UserId)
            join veh in vehicles on vu.VehicleId equals veh.Id into vehJoin
            from veh in vehJoin.DefaultIfEmpty()
            where veh != null
                    select new
            {
                Customer = new CustomerDto { Id = cus.Id, Name = cus.Name, Surname = cus.Surname},
                Vehicle = new VehicleDto
                {
                    Id = veh.Id,
                    Brand = veh.Brand,
                    Model = veh.Model,
                    Plate = veh.Plate,
                    SerialNumber = veh.SerialNumber,
                    Year = veh.Year,
                    Engine = veh.Engine,
                    FuelTypeId = veh.FuelTypeId
                }
            }; 

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(i =>
                    i.Vehicle != null &&
                    (
                        (i.Vehicle.Brand != null && i.Vehicle.Brand.Contains(request.Search)) ||
                        (i.Vehicle.Model != null && i.Vehicle.Model.Contains(request.Search)) ||
                        (i.Vehicle.Plate != null && i.Vehicle.Plate.Contains(request.Search)) ||
                        (i.Vehicle.SerialNumber != null && i.Vehicle.SerialNumber.Contains(request.Search))
                    )
                );

        var result = await query.OrderBy(i => i.Vehicle.Brand)
            .Select(i => new VehicleDto
            {
                Id = i.Vehicle.Id,
                Brand = i.Vehicle.Brand,
                Model = i.Vehicle.Model,
                Plate = i.Vehicle.Plate,
                SerialNumber = i.Vehicle.SerialNumber,
                Year = i.Vehicle.Year,
                Engine = i.Vehicle.Engine,
                FuelTypeId = i.Vehicle.FuelTypeId
            })
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<VehicleDto>>.Success(result);
    }
}