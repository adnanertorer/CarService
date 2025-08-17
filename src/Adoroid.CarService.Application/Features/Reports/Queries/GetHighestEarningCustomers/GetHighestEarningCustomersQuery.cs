using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Reports.Dtos;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Reports.Queries.GetHighestEarningCustomers;

public record GetHighestEarningCustomersQuery : IRequest<Response<List<HighestEarningCustomerDto>>>;

public class GetHighestEarningCustomersQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetHighestEarningCustomersQuery, Response<List<HighestEarningCustomerDto>>>
{
    public async Task<Response<List<HighestEarningCustomerDto>>> Handle(GetHighestEarningCustomersQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var transactions = unitOfWork.AccountTransactions.GetByCompanyId(companyId, true);

        var customerTransactions = transactions
             .Where(t => t.AccountOwnerType == (int)AccountOwnerTypeEnum.Customer || t.AccountOwnerType == (int)AccountOwnerTypeEnum.MobileUser);

        var grouped = await customerTransactions
          .GroupBy(t => t.AccountOwnerId)
          .Select(g => new
          {
              CustomerId = g.Key,
              TotalClaim = g.Sum(x => (decimal?)x.Claim) ?? 0m,
              TotalDebt = g.Sum(x => (decimal?)x.Debt) ?? 0m
          })
          .ToListAsync(cancellationToken);

        var customerIds = grouped.Select(x => x.CustomerId).Distinct().ToList();

        var customers = await unitOfWork.Customers.GetCustomerNames(customerIds, cancellationToken);

        var list = grouped
            .Select(x =>
            {
                customers.TryGetValue(x.CustomerId, out var fullName);
                fullName ??= string.Empty;

                var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
                var lastName = nameParts.Length > 1 ? string.Join(' ', nameParts.Skip(1)) : string.Empty;

                return new HighestEarningCustomerDto
                {
                    CustomerId = x.CustomerId,
                    CustomerName = firstName,
                    CustomerSurname = lastName,
                    TotalClaim = x.TotalClaim,
                    TotalDebt = x.TotalDebt
                };
            })
            .OrderByDescending(d => d.TotalDebt)
            .Take(10)
            .ToList();

        return Response<List<HighestEarningCustomerDto>>.Success(list);
    }
}
