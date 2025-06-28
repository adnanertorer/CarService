namespace Adoroid.CarService.Application.Features.Companies.Dtos;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string AuthorizedName { get; set; }
    public string AuthorizedSurname { get; set; }
    public string TaxNumber { get; set; }
    public string TaxOffice { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public string CompanyAddress { get; set; }
    public string CompanyPhone { get; set; }
    public string CompanyEmail { get; set; }

    public CityDto City { get; set; }
    public DistrictDto District { get; set; }
    public List<CompanyServiceDto> CompanyServices { get; set; } = new List<CompanyServiceDto>();
}
