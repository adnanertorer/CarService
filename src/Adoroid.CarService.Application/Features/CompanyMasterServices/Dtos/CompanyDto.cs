namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string AuthorizedName { get; set; }
    public string AuthorizedSurname { get; set; }
    public string CompanyPhone { get; set; }
}
