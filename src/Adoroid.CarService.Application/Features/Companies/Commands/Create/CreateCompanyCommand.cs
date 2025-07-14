using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using Adoroid.CarService.Application.Features.Companies.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Commands.Create;

public record CreateCompanyCommand(string CompanyName, string AuthorizedName, string AuthorizedSurname,
    string TaxNumber, string TaxOffice, int CityId, int DistrictId, string CompanyAddress,
    string CompanyPhone, string CompanyEmail) : IRequest<Response<CompanyDto>>;

public class CreateCompanyCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCompanyCommand, Response<CompanyDto>>
{
    public async Task<Response<CompanyDto>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var isExist = await unitOfWork.Companies.IsCompanyExistsAsync(request.TaxNumber, request.CompanyEmail, cancellationToken);

        if (isExist)
            return Response<CompanyDto>.Fail(BusinessExceptionMessages.CompanyAlreadyExists);

        var company = new Company
        {
            AuthorizedName = request.AuthorizedName,
            AuthorizedSurname = request.AuthorizedSurname,
            CityId = request.CityId,
            CompanyAddress = request.CompanyAddress,
            CompanyEmail = request.CompanyEmail,
            CompanyName = request.CompanyName,
            CompanyPhone = request.CompanyPhone,
            CreatedBy = Guid.NewGuid(), 
            CreatedDate = DateTime.UtcNow,
            DistrictId = request.DistrictId,
            IsDeleted = false,
            TaxNumber = request.TaxNumber,
            TaxOffice = request.TaxOffice
        };

        await unitOfWork.Companies.AddAsync(company, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<CompanyDto>.Success(company.FromEntity());
    }
}
