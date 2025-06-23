using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.MapperExtensions;

public static class CompanyServiceMappingExtensions
{
    public static CompanyServiceDto FromEntity(this CompanyService companyService)
    {
        return new CompanyServiceDto
        {
            Company = companyService.Company != null ? new CompanyDto
            {
                AuthorizedName = companyService.Company.AuthorizedName,
                AuthorizedSurname = companyService.Company.AuthorizedSurname,
                CompanyName = companyService.Company.CompanyName,
                CompanyPhone = companyService.Company.CompanyPhone,
                Id = companyService.Company.Id
            } : null,
            CompanyId = companyService.CompanyId,
            Id = companyService.Id,
            MasterService = companyService.MasterService != null ? new MasterServiceDto
            {
                Id = companyService.MasterService.Id,
                ServiceName = companyService.MasterService.ServiceName
            } : null,
            MasterServiceId = companyService.MasterServiceId
        };
    }
}
