using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.BusinessMessages;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.MapperExtensions;
using Adoroid.CarService.Application.Features.Users.Queries.CheckCompanyId;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Commands.Create;
public record CreateClaimCommand(Guid CustomerId, AccountOwnerTypeEnum AccountOwnerType, decimal Claim, DateTime TransactionDate, string? Description) 
    : IRequest<Response<AccountTransactionDto>>;

public class CreateClaimCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, IMediator mediator)
    : IRequestHandler<CreateClaimCommand, Response<AccountTransactionDto>>
{
    public async Task<Response<AccountTransactionDto>> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var companyIdResponse = await mediator.Send(new GetCompanyIdCommand(), cancellationToken);

        if (!companyIdResponse.Succeeded)
            return Response<AccountTransactionDto>.Fail(BusinessMessages.CompanyNotFound);

        var companyId = companyIdResponse.Data!.Value;

        var balance = await GetBalance(request.CustomerId, cancellationToken);

        var entity = new AccountingTransaction
        {
            CreatedBy = Guid.Parse(currentUser.Id!),
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false,
            Claim = request.Claim,
            Debt = 0,
            TransactionDate = request.TransactionDate,
            Balance = balance - request.Claim,
            AccountOwnerId = request.CustomerId,
            AccountOwnerType = (int)request.AccountOwnerType,
            CompanyId = companyId,
            TransactionType = (int)TransactionTypeEnum.Receivable,
            Description = request.Description
        };

        await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<AccountTransactionDto>.Success(entity.FromEntity());
    }

    private async Task<decimal> GetBalance(Guid customerId, CancellationToken cancellationToken)
    {
        var totalDebt = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == Guid.Parse(currentUser.CompanyId!))
            .SumAsync(i => i.Debt, cancellationToken);

        var totalClaim = await dbContext.AccountingTransactions.AsNoTracking()
            .Where(i => i.AccountOwnerId == customerId && i.CompanyId == Guid.Parse(currentUser.CompanyId!))
            .SumAsync(i => i.Claim, cancellationToken);

        return Math.Abs(totalDebt - totalClaim);
    }
}

