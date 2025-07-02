using Adoroid.CarService.Application.Common.Enums;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Dtos;

public class UserToCompanyDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public CompanyUserTypeEnum UserType { get; set; }

    public CompanyDto? Company { get; set; }

}
