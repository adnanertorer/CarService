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
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (customer is null)
            return Response<CustomerDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<CustomerDto>.Success(customer.FromEntity());
    }
}
