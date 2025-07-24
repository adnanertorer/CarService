using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Update;

public record UpdateSupplierCommand(Guid Id, string Name, string ContactName, string PhoneNumber, string? Email, string? Address,
    int CityId,
    int DistrictId, string TaxOffice, string TaxNumber) :
    IRequest<Response<SupplierDto>>;

public class UpdateSupplierCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdateSupplierCommand, Response<SupplierDto>>
{
    public async Task<Response<SupplierDto>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await unitOfWork.Suppliers.GetByIdAsync(request.Id.ToString(), asNoTracking: false, cancellationToken);
        if (supplier is null)
            return Response<SupplierDto>.Fail(BusinessExceptionMessages.SupplierNotFound);

        supplier.PhoneNumber = request.PhoneNumber;
        supplier.Address = request.Address;
        supplier.UpdatedDate = DateTime.UtcNow;
        supplier.UpdatedBy = Guid.Parse(currentUser.Id!);
        supplier.ContactName = request.ContactName;
        supplier.Email = request.Email;
        supplier.Name = request.Name;
        supplier.CityId = request.CityId;
        supplier.DistrictId = request.DistrictId;
        supplier.TaxOffice = request.TaxOffice;
        supplier.TaxNumber = request.TaxNumber;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<SupplierDto>.Success(supplier.FromEntity());
    }
}
