using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Application.Features.Customers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Customers.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Commands.Update;

public record UpdateCustomerCommand(Guid Id, string Name, string Surname, string? Email, string Phone, string? Address,
    string? TaxNumber, string? TaxOffice, bool IsActive) : IRequest<Response<CustomerDto>>;

public class UpdateCustomerCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<UpdateCustomerCommand,
    Response<CustomerDto>>
{
    public async Task<Response<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .Include(i => i.Vehicles)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (customer is null)
            return Response<CustomerDto>.Fail(BusinessExceptionMessages.NotFound);

        customer.Surname = request.Surname;
        customer.Address = request.Address;
        customer.TaxNumber = request.TaxNumber;
        customer.UpdatedDate = DateTime.UtcNow;
        customer.UpdatedBy = Guid.Parse(currentUser.Id!);
        customer.Address = request.Address;
        customer.Phone = request.Phone;
        customer.Email = request.Email;
        customer.IsActive = request.IsActive;
        customer.Name = request.Name;
        customer.TaxOffice = request.TaxOffice;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<CustomerDto>.Success(customer.FromEntity());
    }
}
