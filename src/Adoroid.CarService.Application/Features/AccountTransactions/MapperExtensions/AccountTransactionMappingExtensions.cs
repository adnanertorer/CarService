using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.AccountTransactions.MapperExtensions;

public static class AccountTransactionMappingExtensions
{
    public static AccountTransactionDto FromEntity(this AccountingTransaction accountingTransaction)
    {
        return new AccountTransactionDto
        {
            Balance = accountingTransaction.Balance,
            Claim = accountingTransaction.Claim,
            Customer = accountingTransaction.Customer != null ? new CustomerDto
            {
                Id = accountingTransaction.Customer.Id,
                Name = accountingTransaction.Customer.Name,
                Surname = accountingTransaction.Customer.Surname
            } : null,
            CustomerId = accountingTransaction.CustomerId,
            Debt = accountingTransaction.Debt,
            TransactionDate = accountingTransaction.TransactionDate,
            TransactionType = accountingTransaction.TransactionType,
            Description = accountingTransaction.Description
        };
    }
}
