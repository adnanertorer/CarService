using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Suppliers.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Delete;

public record DeleteSupplierCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteSupplierCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<DeleteSupplierCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await dbContext.Suppliers.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (supplier is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.SupplierNotFound);

        supplier.IsDeleted = true;
        supplier.DeletedDate = DateTime.Now;
        supplier.DeletedBy = Guid.Parse(currentUser.Id!);

        dbContext.Suppliers.Update(supplier);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(supplier.Id);
    }
}
