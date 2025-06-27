namespace Adoroid.CarService.Application.Features.SubServices.Dtos;

public class MobileSubServiceDto
{
    public Guid Id { get; set; }
    public Guid MainServiceId { get; set; }
    public string Operation { get; set; }
    public DateTime OperationDate { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public string? MaterialBrand { get; set; }
    public decimal? Discount { get; set; }
    public decimal Cost { get; set; }

    public MainServiceDto? MainService { get; set; }
}
