using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Commands.Create;
public record CreateClaimCommand(Guid CustomerId, AccountOwnerTypeEnum AccountOwnerType, decimal Claim, DateTime TransactionDate, string? Description) 
    : IRequest<Response<AccountTransactionDto>>;

public class CreateClaimCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<CreateClaimCommand, Response<AccountTransactionDto>>
{
    public async Task<Response<AccountTransactionDto>> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var balance = await unitOfWork.AccountTransactions.GetBalanceAsync(request.CustomerId, companyId, cancellationToken);

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

        await unitOfWork.AccountTransactions.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<AccountTransactionDto>.Success(entity.FromEntity());
    }
}

