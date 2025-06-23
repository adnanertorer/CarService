namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;

public class CompanyServiceDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid MasterServiceId { get; set; }

    public CompanyDto? Company { get; set; }
    public MasterServiceDto? MasterService { get; set; }
}
