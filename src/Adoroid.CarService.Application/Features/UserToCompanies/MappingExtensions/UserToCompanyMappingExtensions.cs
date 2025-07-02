using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.UserToCompanies.MappingExtensions;

public static class UserToCompanyMappingExtensions
{
    public static UserToCompanyDto FromEntity(this UserToCompany userToCompany)
    {
        return new UserToCompanyDto
        {
            Id = userToCompany.Id,
            UserId = userToCompany.UserId,
            CompanyId = userToCompany.CompanyId,
            UserType = (CompanyUserTypeEnum)userToCompany.UserType,
            Company = userToCompany.Company?.CompanyFromEntity()
        };
    }

    public static CompanyDto CompanyFromEntity(this Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            AuthorizedName = company.AuthorizedName,
            AuthorizedSurname = company.AuthorizedSurname,
            CompanyAddress = company.CompanyAddress,
            CompanyEmail = company.CompanyEmail,
            CompanyName = company.CompanyName,
            CompanyPhone = company.CompanyPhone
        };
    }
}
