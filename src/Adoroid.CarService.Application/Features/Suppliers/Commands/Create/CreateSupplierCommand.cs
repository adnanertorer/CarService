using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.BusinessMessages;
using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Application.Features.Suppliers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;
using Adoroid.CarService.Application.Features.Users.Queries.CheckCompanyId;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.Application.Features.Suppliers.Commands.Create;

public record CreateSupplierCommand(string Name, string ContactName, string PhoneNumber, string? Email, string? Address):
    IRequest<Response<SupplierDto>>;

public class CreateSupplierCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, IMediator mediator) : IRequestHandler<CreateSupplierCommand, Response<SupplierDto>>
{
    public async Task<Response<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var companyIdResponse = await mediator.Send(new GetCompanyIdCommand(), cancellationToken);

        if (!companyIdResponse.Succeeded)
            return Response<SupplierDto>.Fail(BusinessMessages.CompanyNotFound);

        var companyId = companyIdResponse.Data!.Value;

        var isExist = await dbContext.Suppliers
            .AsNoTracking()
            .AnyAsync(i => i.Name == request.Name || i.ContactName == request.ContactName || i.PhoneNumber == request.PhoneNumber, cancellationToken);

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

        var entity = await dbContext.Suppliers.AddAsync(supplier, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<SupplierDto>.Success(entity.Entity.FromEntity());
    }
}

