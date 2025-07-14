using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Application.Features.Customers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Customers.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Commands.Update;

public record UpdateCustomerCommand(Guid Id, string Name, string Surname, string? Email, string Phone, string? Address,
    string? TaxNumber, string? TaxOffice, bool IsActive, int CityId, int DistrictId) : IRequest<Response<CustomerDto>>;

public class UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdateCustomerCommand,
    Response<CustomerDto>>
{
    public async Task<Response<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetWithVehicleUsersAsync(request.Id, cancellationToken);

        if (customer.Customer is null)
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

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<CustomerDto>.Success(customer.Customer.FromEntity());
    }
}
