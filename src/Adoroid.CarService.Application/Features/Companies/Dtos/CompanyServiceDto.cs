namespace Adoroid.CarService.Application.Features.Companies.Dtos;

public class CompanyServiceDto
{
    public Guid CompanyId { get; set; }
    public Guid MasterServiceId { get; set; }
    public string ServiceName { get; set; }
}
