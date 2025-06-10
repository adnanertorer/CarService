using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Suppliers.Queries.GetById;

public record SupplierGetByIdRequest(Guid Id) : IRequest<Response<SupplierDto>>;

public class SupplierGetByIdRequestHandler(CarServiceDbContext dbContext) : IRequestHandler<SupplierGetByIdRequest, Response<SupplierDto>>
{
    public async Task<Response<SupplierDto>> Handle(SupplierGetByIdRequest request, CancellationToken cancellationToken)
    {
        var supplier = await dbContext.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (supplier is null)
            return Response<SupplierDto>.Fail(BusinessExceptionMessages.SupplierNotFound);

        return Response<SupplierDto>.Success(supplier.FromEntity());
    }
}
