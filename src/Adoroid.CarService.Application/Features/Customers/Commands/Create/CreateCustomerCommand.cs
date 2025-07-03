using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Application.Features.Customers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Customers.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Commands.Create;

public record CreateCustomerCommand(string Name, string Surname, string? Email, string Phone, string? Address,
    string? TaxNumber, string? TaxOffice, bool IsActive) : IRequest<Response<CustomerDto>>;

public class CreateCustomerCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<CreateCustomerCommand,
    Response<CustomerDto>>
{
    public async Task<Response<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var isExist = await dbContext.Customers.AsNoTracking()
            .AnyAsync(i => i.Name == request.Name && 
            i.Surname == request.Surname && 
            i.CompanyId == companyId, cancellationToken);

        if (isExist)
            return Response<CustomerDto>.Fail(BusinessExceptionMessages.NotFound);

        var customer = new Customer
        {
            Address = request.Address,
            CompanyId = companyId,
            CreatedBy = Guid.Parse(currentUser.Id!),
            Email = request.Email,
            IsActive = request.IsActive,
            Name = request.Name,
            Phone = request.Phone,
            Surname = request.Surname,
            TaxNumber = request.TaxNumber,
            TaxOffice = request.TaxOffice
        };

        var entityResult = await dbContext.Customers.AddAsync(customer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<CustomerDto>.Success(entityResult.Entity.FromEntity());
    }
}
