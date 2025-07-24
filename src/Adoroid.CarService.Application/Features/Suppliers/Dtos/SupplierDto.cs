namespace Adoroid.CarService.Application.Features.Suppliers.Dtos;

public class SupplierDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public string? TaxNumber { get; set; }
    public string? TaxOffice { get; set; }

    public CityModel? City { get; set; }
    public DistrictModel? District { get; set; }
}
