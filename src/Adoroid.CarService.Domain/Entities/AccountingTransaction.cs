using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class AccountingTransaction : Entity<Guid>
{
    public Guid AccountOwnerId { get; set; }
    public int AccountOwnerType { get; set; } // "customer" or "mobileUser"
    public Guid CompanyId { get; set; }
    public int TransactionType { get; set; } // 0: Income, 1: Expense
    public decimal Claim { get; set; } // Claim amount for expenses
    public decimal Debt { get; set; } // Debt amount for incomes
    public decimal Balance { get; set; } // Balance after the transaction
    public DateTime TransactionDate { get; set; } // Date of the transaction
    public string? Description { get; set; }

    public Company? Company { get; set; } 

}
