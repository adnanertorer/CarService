namespace Adoroid.CarService.Application.Features.AccountTransactions.Dtos;

public class CustomerDebtDto
{
    public Guid CustomerId { get; set; } 
    public string CustomerName { get; set; } 
    public string CustomerSurname { get; set; } 
    public decimal Balance { get; set; } 
    public List<AccountTransactionDto> Transactions { get; set; } = new List<AccountTransactionDto>();
}
