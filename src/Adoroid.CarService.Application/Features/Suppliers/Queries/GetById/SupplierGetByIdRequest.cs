using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Suppliers.Queries.GetById;

public record SupplierGetByIdRequest(Guid Id) : IRequest<Response<SupplierDto>>;

public class SupplierGetByIdRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<SupplierGetByIdRequest, Response<SupplierDto>>
{
    public async Task<Response<SupplierDto>> Handle(SupplierGetByIdRequest request, CancellationToken cancellationToken)
    {
        var supplier = await unitOfWork.Suppliers.GetByIdWithIncludesAsync(request.Id.ToString(), asNoTracking: true, cancellationToken);

        if (supplier is null)
            return Response<SupplierDto>.Fail(BusinessExceptionMessages.SupplierNotFound);

        return Response<SupplierDto>.Success(supplier.FromEntity());
    }
}
