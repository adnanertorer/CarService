using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Filters;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetList;

public record GetListAccountTransactionRequest(MainFilterRequestModel MainFilterRequest)
    : IRequest<Response<Paginate<AccountTransactionDto>>>;

public class GetListAccountTransactionRequestHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetListAccountTransactionRequest, Response<Paginate<AccountTransactionDto>>>
{

    public async Task<Response<Paginate<AccountTransactionDto>>> Handle(GetListAccountTransactionRequest request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var transactionsQuery = unitOfWork.AccountTransactions.GetByCompanyId(companyId, asNoTracking: true);

        if (request.MainFilterRequest.CustomerId != null)
            transactionsQuery = transactionsQuery.Where(i => i.AccountOwnerId == request.MainFilterRequest.CustomerId);

        if (request.MainFilterRequest.StartDate.HasValue && request.MainFilterRequest.EndDate.HasValue)
        {
            transactionsQuery = transactionsQuery.WhereTwoDateIsBetween(
                i => i.TransactionDate,
                request.MainFilterRequest.StartDate.Value.Date,
                request.MainFilterRequest.EndDate.Value.Date.AddDays(1));
        }
        else if (request.MainFilterRequest.StartDate.HasValue)
        {
            transactionsQuery = transactionsQuery.WhereDateIsBetween(i => i.TransactionDate, request.MainFilterRequest.StartDate.Value);
        }


        var transactions = transactionsQuery
           .OrderBy(i => i.TransactionDate);

        var customerIds = transactions
            .Where(t => t.AccountOwnerType == (int)AccountOwnerTypeEnum.Customer)
            .Select(t => t.AccountOwnerId)
            .Distinct()
            .ToList();

        var mobileUserIds = transactions
            .Where(t => t.AccountOwnerType == (int)AccountOwnerTypeEnum.MobileUser)
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

        if (!string.IsNullOrEmpty(request.MainFilterRequest.Search))
            dtoItems = dtoItems.Where(i => i.OwnerName.Equals(request.MainFilterRequest.Search)).ToList();

        var result = dtoItems.AsQueryable()
            .OrderByDescending(i => i.TransactionDate)
            .ToPaginate(request.MainFilterRequest.PageRequest.PageIndex, request.MainFilterRequest.PageRequest.PageSize);

        return Response<Paginate<AccountTransactionDto>>.Success(result);
    }
}

