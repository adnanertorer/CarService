using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Reports.Dtos;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Reports.Queries.GetTransactions;

public record GetTransactionQuery : IRequest<Response<List<TransactionReportDto>>>;

public class GetTransactionQueryHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork) : IRequestHandler<GetTransactionQuery, Response<List<TransactionReportDto>>>
{
    public async Task<Response<List<TransactionReportDto>>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var transactionsQuery = unitOfWork.AccountTransactions.GetByCompanyId(companyId, asNoTracking: true);

        var totals = await transactionsQuery
            .GroupBy(_ => 1)
            .Select(g => new
            {
                TotalDebt = g.Where(i => i.TransactionType == (int)TransactionTypeEnum.Payable
                                      || i.TransactionType == (int)TransactionTypeEnum.Adjustment)
                             .Sum(i => i.Debt),
                TotalClaim = g.Where(i => i.TransactionType == (int)TransactionTypeEnum.Receivable)
                              .Sum(i => i.Claim)
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalDebt = 0m, TotalClaim = 0m };


        var list = new List<TransactionReportDto>
        {
            new TransactionReportDto
            {
                TransactionTypeId = ((int)TransactionTypeEnum.Payable).ToString(),
                TransactionTypeName = "Toplam Alacak",
                Amount = totals.TotalDebt,
                Fill = $"var(--color-{((int)TransactionTypeEnum.Payable).ToString()})"
            },
            new TransactionReportDto
            {
                TransactionTypeId = ((int)TransactionTypeEnum.Receivable).ToString(),
                TransactionTypeName = "Toplam Tahsilat",
                Amount = totals.TotalClaim,
                Fill = $"var(--color-{((int)TransactionTypeEnum.Receivable).ToString()})"
            },
            new TransactionReportDto
            {
                TransactionTypeId = "Balance",
                TransactionTypeName = "Kalan Alacak",
                Amount = totals.TotalDebt - totals.TotalClaim,
                Fill = "var(--color-Balance)"
            }
        };


        return Response<List<TransactionReportDto>>.Success(list);
    }
}
