using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetTransactionTotals;

public record GetTransactionTotalsQuery(Guid? CustomerId, DateTime? StartDate, DateTime? EndDate) : IRequest<Response<TransactionTotalDto>>;

public class GetTransactionTotalsQueryHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork) : IRequestHandler<GetTransactionTotalsQuery, Response<TransactionTotalDto>>
{
    public async Task<Response<TransactionTotalDto>> Handle(GetTransactionTotalsQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var transactionsQuery = unitOfWork.AccountTransactions.GetByCompanyId(companyId, asNoTracking: true);

        if (request.CustomerId != null)
            transactionsQuery = transactionsQuery.Where(i => i.AccountOwnerId == request.CustomerId);

        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            transactionsQuery = transactionsQuery.WhereTwoDateIsBetween(
                i => i.TransactionDate,
                request.StartDate.Value.Date,
                request.EndDate.Value.Date.AddDays(1));
        }
        else if (request.StartDate.HasValue)
        {
            transactionsQuery = transactionsQuery.Where(i => i.TransactionDate >= request.StartDate.Value.Date);
        }
        else if (request.EndDate.HasValue)
        {
            transactionsQuery = transactionsQuery.Where(i => i.TransactionDate < request.EndDate.Value.Date.AddDays(1));
        }

        var totals = await transactionsQuery
            .GroupBy(_ => 1) // Tek grup oluşturmak için
            .Select(g => new
            {
                TotalDebt = g.Where(i => i.TransactionType == (int)TransactionTypeEnum.Payable
                                      || i.TransactionType == (int)TransactionTypeEnum.Adjustment)
                             .Sum(i => i.Debt),
                TotalClaim = g.Where(i => i.TransactionType == (int)TransactionTypeEnum.Receivable)
                              .Sum(i => i.Claim)
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalDebt = 0m, TotalClaim = 0m };


        return Response<TransactionTotalDto>.Success(new TransactionTotalDto
        {
            TotalDebt = totals.TotalDebt,
            TotalClaim = totals.TotalClaim,
            Total = totals.TotalDebt - totals.TotalClaim
        });
    }
}