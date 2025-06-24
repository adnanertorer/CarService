using Adoroid.CarService.Application.Features.AccountTransactions.Dtos;
using Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;
using Adoroid.CarService.Application.Features.AccountTransactions.MapperExtensions;
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
        var entity = await dbContext.AccountingTransactions
            .Include(i => i.Customer)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<AccountTransactionDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<AccountTransactionDto>.Success(entity.FromEntity());
    }
}

