using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Queries.GetById;

public record GetByIdAccountTransactionRequest(Guid Id) : IRequest<Response<AccountTransactionDto>>;

public class GetByIdAccountTransactionRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetByIdAccountTransactionRequest, Response<AccountTransactionDto>>
{

    public async Task<Response<AccountTransactionDto>> Handle(GetByIdAccountTransactionRequest request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.AccountTransactions.GetByIdAsync(request.Id, true, cancellationToken);

        if (result is null)
            return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.NotFound);

        string ownerName = string.Empty;
        if(result.AccountOwnerType == (int)AccountOwnerTypeEnum.Customer)
        {
            ownerName = await unitOfWork.Customers.GetNameByIdAsync(result.AccountOwnerId, cancellationToken);
        }
        else
        {
            ownerName = await unitOfWork.MobileUsers.GetNameById(result.AccountOwnerId, cancellationToken);
        }

        var dto = new AccountTransactionDto
        {
            OwnerName = ownerName,
            Id = result.Id,
            AccountOwnerId = result.AccountOwnerId,
            AccountOwnerType = result.AccountOwnerType,
            Debt = result.Debt,
            Claim = result.Claim,
            Balance = result.Balance,
            TransactionDate = result.TransactionDate,
            Description = result.Description,
            TransactionType = result.TransactionType
        };

        return Response<AccountTransactionDto>.Success(dto);
    }
}

