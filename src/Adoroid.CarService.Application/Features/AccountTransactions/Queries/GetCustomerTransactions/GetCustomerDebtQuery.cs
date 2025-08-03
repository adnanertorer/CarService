using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetCustomerTransactions;

public record GetCustomerDebtQuery(Guid CustomerId) : IRequest<Response<CustomerDebtDto>>;

public class GetCustomerDebtQueryHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork) : IRequestHandler<GetCustomerDebtQuery, Response<CustomerDebtDto>>
{
    public async Task<Response<CustomerDebtDto>> Handle(GetCustomerDebtQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var customer = await unitOfWork.Customers.GetByIdAsync(request.CustomerId, true, cancellationToken);

        if (customer is null)
            return Response<CustomerDebtDto>.Fail(BusinessExceptionMessages.CustomerNotFound);

        var balance = await unitOfWork.AccountTransactions.GetBalanceAsync(request.CustomerId, companyId, cancellationToken);

        var transactions = unitOfWork.AccountTransactions.GetByCustomerId(companyId, request.CustomerId, true);

        var customerIds = transactions
            .Where(t => t.AccountOwnerType == (int)AccountOwnerTypeEnum.Customer && t.AccountOwnerId == request.CustomerId)
            .Select(t => t.AccountOwnerId)
            .Distinct()
            .ToList();

        var mobileUserIds = transactions
            .Where(t => t.AccountOwnerType == (int)AccountOwnerTypeEnum.MobileUser && t.AccountOwnerId == request.CustomerId)
            .Select(t => t.AccountOwnerId)
            .Distinct()
            .ToList();

        var customers = await unitOfWork.Customers.GetCustomerNames(customerIds, cancellationToken);

        var mobileUsers = await unitOfWork.MobileUsers.GetUserNames(mobileUserIds, cancellationToken);

        var dtoItems = transactions.AsEnumerable().Select(t => new AccountTransactionDto
        {
            Id = t.Id,
            AccountOwnerId = t.AccountOwnerId,
            AccountOwnerType = t.AccountOwnerType,
            OwnerName = t.AccountOwnerType == (int)AccountOwnerTypeEnum.Customer
                       ? customers.TryGetValue(t.AccountOwnerId, out var cName) ? cName : string.Empty
                       : mobileUsers.TryGetValue(t.AccountOwnerId, out var mName) ? mName : string.Empty,
            Debt = t.Debt,
            Claim = t.Claim,
            Balance = t.Balance,
            TransactionDate = t.TransactionDate,
            Description = t.Description,
            TransactionType = t.TransactionType,
            CompanyId = companyId
        }).ToList();

        return Response<CustomerDebtDto>.Success(new CustomerDebtDto
        {
            CustomerId = request.CustomerId,
            CustomerName = customer.Name,
            CustomerSurname = customer.Surname,
            Balance = balance,
            Transactions = dtoItems
        });
    }
}  
