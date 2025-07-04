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
    string? TaxNumber, string? TaxOffice, bool IsActive, int CityId, int DistrictId) : IRequest<Response<CustomerDto>>;

public class UpdateCustomerCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<UpdateCustomerCommand,
    Response<CustomerDto>>
{
    public async Task<Response<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .Select(i => new
            {
                Customer = i,
                VehicleUsers = i.VehicleUsers
                     .Where(j => (i.MobileUserId != null && j.UserId == i.MobileUserId) || j.UserId == i.Id)
                     .Select(vu => new
                     {
                         VehicleUser = vu,
                         Vehicle = vu.Vehicle
                     })
            })
            .FirstOrDefaultAsync(i => i.Customer.Id == request.Id, cancellationToken);

        if (customer is null)
            return Response<CustomerDto>.Fail(BusinessExceptionMessages.NotFound);

        customer.Customer.Surname = request.Surname;
        customer.Customer.Address = request.Address;
        customer.Customer.TaxNumber = request.TaxNumber;
        customer.Customer.UpdatedDate = DateTime.UtcNow;
        customer.Customer.UpdatedBy = Guid.Parse(currentUser.Id!);
        customer.Customer.Address = request.Address;
        customer.Customer.Phone = request.Phone;
        customer.Customer.Email = request.Email;
        customer.Customer.IsActive = request.IsActive;
        customer.Customer.Name = request.Name;
        customer.Customer.TaxOffice = request.TaxOffice;
        customer.Customer.CityId = request.CityId;
        customer.Customer.DistrictId = request.DistrictId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<CustomerDto>.Success(customer.Customer.FromEntity());
    }
}
