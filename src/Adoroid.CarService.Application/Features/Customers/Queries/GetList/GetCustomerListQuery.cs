using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Queries.GetList;

public record GetCustomerListQuery(PageRequest PageRequest, string? Search) : IRequest<Response<Paginate<CustomerDto>>>;

public class GetCustomerListQueryHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<GetCustomerListQuery, Response<Paginate<CustomerDto>>>
{
    public async Task<Response<Paginate<CustomerDto>>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var query = from customer in dbContext.Customers
                join vehicleUser in dbContext.VehicleUsers
                    on customer.Id equals vehicleUser.UserId into vehicleUsersGroup
                from vu in vehicleUsersGroup.DefaultIfEmpty()
                join vehicle in dbContext.Vehicles
                    on vu.VehicleId equals vehicle.Id into vehiclesGroup
                from v in vehiclesGroup.DefaultIfEmpty()
                where customer.CompanyId == companyId &&
                      (
                          (customer.MobileUserId != null && vu.UserId == customer.MobileUserId) ||
                          vu.UserId == customer.Id
                      )
                select new
                {
                    Customer = customer,
                    VehicleUser = vu,
                    Vehicle = v
                };

        if (!string.IsNullOrWhiteSpace(request.Search)) 
            query = query.Where(i => i.Customer.Name.Contains(request.Search) || i.Customer.Surname.Contains(request.Search) || i.Customer.Phone.Contains(request.Search));


        var result = await query
            .GroupBy(i => i.Customer)
            .Select(g => new CustomerDto
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                Vehicles = g
                    .Where(x => x.Vehicle != null)
                    .Select(x => new VehicleDto
                    {
                        Id = x.Vehicle.Id,
                        Plate = x.Vehicle.Plate,
                        Brand = x.Vehicle.Brand,
                        Model = x.Vehicle.Model,
                        Engine = x.Vehicle.Engine,
                        FuelTypeId = x.Vehicle.FuelTypeId,
                        SerialNumber = x.Vehicle.SerialNumber,
                        Year = x.Vehicle.Year
                    })
                    .Distinct()
                    .ToList(),
                Address = g.Key.Address,
                CompanyId = g.Key.CompanyId,
                Email = g.Key.Email,
                IsActive = g.Key.IsActive,
                Phone = g.Key.Phone,
                Surname = g.Key.Surname,
                TaxNumber = g.Key.TaxNumber,
                TaxOffice = g.Key.TaxOffice,
                VehicleUsers = g.Select(i => new VehicleUserDto
                {
                    Id = i.VehicleUser.Id,
                    UserId = i.VehicleUser.UserId,
                    UserTypeId = i.VehicleUser.UserTypeId,
                    VehicleId = i.VehicleUser.VehicleId
                }).ToList()
            })
            .OrderBy(x => x.Name)
            .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<CustomerDto>>.Success(result);
    }
}