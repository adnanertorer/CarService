using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Customers.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Commands.Delete;

public record DeleteCustomerCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteCustomerCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<DeleteCustomerCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
             .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (customer is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        customer.DeletedDate = DateTime.UtcNow;
        customer.DeletedBy = Guid.Parse(currentUser.Id!);
        customer.IsDeleted = true;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(customer.Id);
    }
}