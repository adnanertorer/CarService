using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Update;

public record UpdateSupplierCommand(Guid Id, string Name, string ContactName, string PhoneNumber, string? Email, string? Address) :
    IRequest<Response<SupplierDto>>;

public class UpdateSupplierCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<UpdateSupplierCommand, Response<SupplierDto>>
{
    public async Task<Response<SupplierDto>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await dbContext.Suppliers.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (supplier is null)
            return Response<SupplierDto>.Fail(BusinessExceptionMessages.SupplierNotFound);

        supplier.PhoneNumber = request.PhoneNumber;
        supplier.Address = request.Address;
        supplier.UpdatedDate = DateTime.UtcNow;
        supplier.UpdatedBy = Guid.Parse(currentUser.Id!);
        supplier.ContactName = request.ContactName;
        supplier.Email = request.Email;
        supplier.Name = request.Name;
        
        dbContext.Suppliers.Update(supplier);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<SupplierDto>.Success(supplier.FromEntity());
    }
}
