using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Commands;

public record CreateCompanyCommand(string CompanyName, string AuthorizedName, string AuthorizedSurname,
    string TaxNumber, string TaxOffice, int CityId, int DistrictId, string CompanyAddress,
    string CompanyPhone, string CompanyEmail) : IRequest<Response<CompanyDto>>;

public class CreateCompanyCommandHandler(CarServiceDbContext dbContext) : IRequestHandler<CreateCompanyCommand, Response<CompanyDto>>
{
    public async Task<Response<CompanyDto>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var isExist = await dbContext.Companies.AsNoTracking()
            .AnyAsync(x => x.TaxNumber == request.TaxNumber || x.CompanyEmail == request.CompanyEmail, cancellationToken);

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

        await dbContext.Companies.AddAsync(company, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var companyDto = new CompanyDto
        {
            Id = company.Id,
            AuthorizedName = company.AuthorizedName,
            AuthorizedSurname = company.AuthorizedSurname,
            CityId = company.CityId,
            CompanyAddress = company.CompanyAddress,
            CompanyEmail = company.CompanyEmail,
            CompanyName = company.CompanyName,
            CompanyPhone = company.CompanyPhone,
            DistrictId = company.DistrictId,
            TaxNumber = company.TaxNumber,
            TaxOffice = company.TaxOffice
        };

        return Response<CompanyDto>.Success(companyDto);
    }
}
