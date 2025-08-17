namespace Adoroid.CarService.Application.Features.Reports.Dtos;

public class TransactionReportDto
{
    public string TransactionTypeId { get; set; }
    public string TransactionTypeName { get; set; }
    public decimal Amount { get; set; }
    public string Fill { get; set; }
}

