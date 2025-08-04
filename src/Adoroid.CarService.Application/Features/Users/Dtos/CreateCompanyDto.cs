namespace Adoroid.CarService.Application.Features.Users.Dtos;

public class CreateCompanyDto
{
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
}
