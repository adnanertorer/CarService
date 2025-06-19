namespace Adoroid.CarService.Application.Features.SubServices.Dtos;

public class SubServiceDto
{
    public Guid Id { get; set; }
    public Guid MainServiceId { get; set; }
    public string Operation { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime OperationDate { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public string? MaterialBrand { get; set; }
    public Guid? SupplierId { get; set; }
    public decimal? Discount { get; set; }
    public decimal Cost { get; set; }

    public MainServiceDto MainService { get; set; }
    public EmployeeDto Employee { get; set; }
    public SupplierDto? Supplier { get; set; }
}
