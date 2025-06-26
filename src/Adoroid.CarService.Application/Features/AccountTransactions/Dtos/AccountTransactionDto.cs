using Adoroid.CarService.Application.Common.Enums;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Dtos;

public class AccountTransactionDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Company ID for multi-tenancy support
    public Guid AccountOwnerId { get; set; }
    public AccountOwnerTypeEnum AccountOwnerType { get; set; } // 0: Customer, 1: MobileUser
    public int TransactionType { get; set; } // 0: Income, 1: Expense
    public decimal Claim { get; set; } // Claim amount for expenses
    public decimal Debt { get; set; } // Debt amount for incomes
    public decimal Balance { get; set; } // Balance after the transaction
    public DateTime TransactionDate { get; set; } // Date of the transaction
    public string? Description { get; set; }
    public string OwnerName { get; set; } = string.Empty; // Name of the account owner (Customer or Mobile User)
}
