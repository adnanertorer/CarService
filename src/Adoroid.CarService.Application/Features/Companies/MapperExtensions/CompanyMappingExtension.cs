using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Companies.MapperExtensions;

public static class CompanyMappingExtension
{
    public static CompanyDto FromEntity(this Company entity)
    {
        return new CompanyDto
        {
            AuthorizedName = entity.AuthorizedName,
            AuthorizedSurname = entity.AuthorizedSurname,
            CityId = entity.CityId,
            CompanyAddress = entity.CompanyAddress,
            CompanyEmail = entity.CompanyEmail,
            CompanyName = entity.CompanyName,
            CompanyPhone = entity.CompanyPhone,
            DistrictId = entity.DistrictId,
            Id = entity.Id,
            TaxNumber = entity.TaxNumber,
            TaxOffice = entity.TaxOffice
        };
    }

    public static Company FromModel(this CompanyDto companyDto)
    {
        return new Company
        {
            Id = companyDto.Id,
            AuthorizedName = companyDto.AuthorizedName,
            AuthorizedSurname = companyDto.AuthorizedSurname,
            CityId = companyDto.CityId,
            CompanyAddress = companyDto.CompanyAddress,
            CompanyEmail = companyDto.CompanyEmail,
            CompanyName = companyDto.CompanyName,
            CompanyPhone = companyDto.CompanyPhone,
            DistrictId = companyDto.DistrictId,
            TaxNumber = companyDto.TaxNumber
        };
    }
}
