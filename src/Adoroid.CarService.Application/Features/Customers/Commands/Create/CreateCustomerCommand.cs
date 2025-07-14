using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Application.Features.Customers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Customers.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Customers.Commands.Create;

public record CreateCustomerCommand(string Name, string Surname, string? Email, string Phone, string? Address,
    string? TaxNumber, string? TaxOffice, bool IsActive, int CityId, int DistrictId) : IRequest<Response<CustomerDto>>;

public class CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<CreateCustomerCommand,
    Response<CustomerDto>>
{
    public async Task<Response<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var isExist = await unitOfWork.Customers.IsCustomerExistsAsync(request.Name, request.Surname, companyId, request.Phone, cancellationToken);

        if (isExist)
            return Response<CustomerDto>.Fail(BusinessExceptionMessages.AlreadyExists);

        var customer = new Customer
        {
            Address = request.Address,
            CompanyId = companyId,
            CreatedBy = Guid.Parse(currentUser.Id!),
            CreatedDate = DateTime.UtcNow,
            Email = request.Email,
            IsActive = request.IsActive,
            Name = request.Name,
            Phone = request.Phone,
            Surname = request.Surname,
            TaxNumber = request.TaxNumber,
            TaxOffice = request.TaxOffice,
            IsDeleted = false,
            CityId = request.CityId,
            DistrictId = request.DistrictId
        };

        await unitOfWork.Customers.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<CustomerDto>.Success(customer.FromEntity());
    }
}
