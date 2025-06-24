using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Commands.Delete;

public record DeleteClaimCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteClaimCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<DeleteClaimCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.AccountingTransactions.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        entity.IsDeleted = true;
        entity.DeletedBy = Guid.Parse(currentUser.Id!);
        entity.DeletedDate = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(entity.Id);
    }
}

