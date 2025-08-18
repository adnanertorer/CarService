namespace Adoroid.CarService.Application.Features.Reports.Dtos;

public class HighestEarningCustomerDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerSurname { get; set; }
    public decimal TotalClaim { get; set; }
    public decimal TotalDebt { get; set; }
}
