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
        var query = dbContext.AccountingTransactions.Select(t => new AccountTransactionDto
        {
            OwnerName = t.AccountOwnerType == (int)AccountOwnerTypeEnum.Customer
                                                             ? dbContext.Customers.Where(c => c.Id == t.AccountOwnerId).Select(c => c.Name + " " + c.Surname).FirstOrDefault() ?? string.Empty
                                                             : dbContext.MobileUsers.Where(m => m.Id == t.AccountOwnerId).Select(m => m.Name + " " + m.Surname).FirstOrDefault() ?? string.Empty,
            Id = t.Id,
            AccountOwnerId = t.AccountOwnerId,
            AccountOwnerType = (AccountOwnerTypeEnum)t.AccountOwnerType,
            Debt = t.Debt,
            Claim = t.Claim,
            Balance = t.Balance,
            TransactionDate = t.TransactionDate,
            Description = t.Description,
            TransactionType = t.TransactionType
        })
            .AsNoTracking()
            .Where(i => i.CompanyId == Guid.Parse(currentUser.CompanyId!));

        if (!string.IsNullOrEmpty(request.MainFilterRequest.Search))
            query = query.Where(i => i.OwnerName.Equals(request.MainFilterRequest.Search));

        if (request.MainFilterRequest.CustomerId != null)
            query = query.Where(i => i.AccountOwnerId == request.MainFilterRequest.CustomerId);

        if (request.MainFilterRequest.StartDate.HasValue && request.MainFilterRequest.EndDate.HasValue)
        {
            query = query.WhereTwoDateIsBetween(
                i => i.TransactionDate,
                request.MainFilterRequest.StartDate.Value.Date,
                request.MainFilterRequest.EndDate.Value.Date.AddDays(1));
        }
        else if (request.MainFilterRequest.StartDate.HasValue)
        {
            query = query.WhereDateIsBetween(i => i.TransactionDate, request.MainFilterRequest.StartDate.Value);
        }

        var result = await query
                .OrderBy(i => i.TransactionDate)
                .ToPaginateAsync(request.MainFilterRequest.PageRequest.PageIndex, request.MainFilterRequest.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<AccountTransactionDto>>.Success(result);
    }
}

