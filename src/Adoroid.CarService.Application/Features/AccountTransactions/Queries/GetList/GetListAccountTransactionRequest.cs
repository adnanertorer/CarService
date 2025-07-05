using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Filters;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetList;

public record GetListAccountTransactionRequest(MainFilterRequestModel MainFilterRequest)
    : IRequest<Response<Paginate<AccountTransactionDto>>>;

public class GetListAccountTransactionRequestHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<GetListAccountTransactionRequest, Response<Paginate<AccountTransactionDto>>>
{

    public async Task<Response<Paginate<AccountTransactionDto>>> Handle(GetListAccountTransactionRequest request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var transactionsQuery = dbContext.AccountingTransactions
            .AsNoTracking()
            .Where(i => i.CompanyId == companyId);

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


        var transactions = await transactionsQuery
           .OrderBy(i => i.TransactionDate)
           .ToPaginateAsync(
               request.MainFilterRequest.PageRequest.PageIndex,
               request.MainFilterRequest.PageRequest.PageSize,
               cancellationToken);

        var customerIds = transactions.Items
            .Where(t => t.AccountOwnerType == (int)AccountOwnerTypeEnum.Customer)
            .Select(t => t.AccountOwnerId)
            .Distinct()
            .ToList();

        var mobileUserIds = transactions.Items
            .Where(t => t.AccountOwnerType == (int)AccountOwnerTypeEnum.MobileUser)
            .Select(t => t.AccountOwnerId)
            .Distinct()
            .ToList();

        var customers = await dbContext.Customers
            .Where(c => customerIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => $"{c.Name} {c.Surname}", cancellationToken);

        var mobileUsers = await dbContext.MobileUsers
            .Where(m => mobileUserIds.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id, m => $"{m.Name} {m.Surname}", cancellationToken);

        var dtoItems = transactions.Items.Select(t => new AccountTransactionDto
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

