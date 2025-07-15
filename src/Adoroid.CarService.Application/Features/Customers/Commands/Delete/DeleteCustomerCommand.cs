using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Customers.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Commands.Delete;

public record DeleteCustomerCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteCustomerCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<DeleteCustomerCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetByIdAsync(request.Id, false, cancellationToken);

        if (customer is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        customer.DeletedDate = DateTime.UtcNow;
        customer.DeletedBy = Guid.Parse(currentUser.Id!);
        customer.IsDeleted = true;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(customer.Id);
    }
}