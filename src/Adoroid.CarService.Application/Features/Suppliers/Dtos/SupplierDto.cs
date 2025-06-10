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
}
