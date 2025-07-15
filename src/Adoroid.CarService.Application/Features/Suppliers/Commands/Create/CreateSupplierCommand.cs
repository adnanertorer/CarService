using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Create;

public record CreateSupplierCommand(string Name, string ContactName, string PhoneNumber, string? Email, string? Address):
    IRequest<Response<SupplierDto>>;

public class CreateSupplierCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<CreateSupplierCommand, Response<SupplierDto>>
{
    public async Task<Response<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var isExist = await unitOfWork.Suppliers.IsExist(request.Name, request.ContactName, request.PhoneNumber, cancellationToken);

        if (isExist)
            return Response<SupplierDto>.Fail(BusinessExceptionMessages.SupplierAlreadyExists);

        var supplier = new Supplier
        {
            Address = request.Address,
            CompanyId = companyId,
            Name = request.Name,
            ContactName = request.ContactName,
            CreatedBy = Guid.Parse(currentUser.Id!),
            PhoneNumber = request.PhoneNumber,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow
        };

        await unitOfWork.Suppliers.AddAsync(supplier, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<SupplierDto>.Success(supplier.FromEntity());
    }
}

