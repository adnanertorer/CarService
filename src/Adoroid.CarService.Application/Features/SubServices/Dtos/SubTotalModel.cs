namespace Adoroid.CarService.Application.Features.SubServices.Dtos;

public class SubTotalModel
{
    public decimal? TotalMaterialCost { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalPrice { get; set; }
}
