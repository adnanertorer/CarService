namespace Adoroid.CarService.Application.Features.AccountTransactions.Dtos;

public class AccountTransactionDto
{
    public Guid CustomerId { get; set; }
    public int TransactionType { get; set; } // 0: Income, 1: Expense
    public decimal Claim { get; set; } // Claim amount for expenses
    public decimal Debt { get; set; } // Debt amount for incomes
    public decimal Balance { get; set; } // Balance after the transaction
    public DateTime TransactionDate { get; set; } // Date of the transaction
    public string? Description { get; set; }

    public CustomerDto? Customer { get; set; }
}
