using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Suppliers.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Delete;

public record DeleteSupplierCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteSupplierCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<DeleteSupplierCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await unitOfWork.Suppliers.GetByIdAsync(request.Id.ToString(), asNoTracking: false, cancellationToken);
        if (supplier is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.SupplierNotFound);

        supplier.IsDeleted = true;
        supplier.DeletedDate = DateTime.UtcNow;
        supplier.DeletedBy = Guid.Parse(currentUser.Id!);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(supplier.Id);
    }
}
