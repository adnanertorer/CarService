using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Application.Features.Customers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Customers.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Queries.GetById;

public record CustomerGetByIdQuery(Guid Id) : IRequest<Response<CustomerDto>>;

public class CustomerGetByIdQueryHandler(CarServiceDbContext dbContext) : IRequestHandler<CustomerGetByIdQuery, Response<CustomerDto>>
{
    public async Task<Response<CustomerDto>> Handle(CustomerGetByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
             .Select(i => new
             {
                 Customer = i,
                 VehicleUsers = i.VehicleUsers
                     .Where(j => (i.MobileUserId != null && j.UserId == i.MobileUserId) || j.UserId == i.Id)
                     .Select(vu => new
                     {
                         VehicleUser = vu,
                         Vehicle = vu.Vehicle
                     })
             })
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Customer.Id == request.Id, cancellationToken);

        if (customer is null)
            return Response<CustomerDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<CustomerDto>.Success(customer.Customer.FromEntity());
    }
}
