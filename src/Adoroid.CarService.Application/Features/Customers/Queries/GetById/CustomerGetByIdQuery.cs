using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Application.Features.Customers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Customers.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Queries.GetById;

public record CustomerGetByIdQuery(Guid Id) : IRequest<Response<CustomerDto>>;

public class CustomerGetByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CustomerGetByIdQuery, Response<CustomerDto>>
{
    public async Task<Response<CustomerDto>> Handle(CustomerGetByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetByIdWithIncludesAsync(request.Id, true, cancellationToken);

        if (customer is null)
            return Response<CustomerDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<CustomerDto>.Success(customer.FromEntity());
    }
}
