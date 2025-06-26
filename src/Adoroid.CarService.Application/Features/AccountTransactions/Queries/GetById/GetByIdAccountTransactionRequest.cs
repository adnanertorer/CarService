using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetById;

public record GetByIdAccountTransactionRequest(Guid Id) : IRequest<Response<AccountTransactionDto>>;

public class GetByIdAccountTransactionRequestHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetByIdAccountTransactionRequest, Response<AccountTransactionDto>>
{

    public async Task<Response<AccountTransactionDto>> Handle(GetByIdAccountTransactionRequest request, CancellationToken cancellationToken)
    {
        var result = await dbContext.AccountingTransactions
            .AsNoTracking()
             .Select(t => new AccountTransactionDto
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
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (result is null)
            return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<AccountTransactionDto>.Success(result);
    }
}

