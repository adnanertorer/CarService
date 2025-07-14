using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.AccountTransactions.Commands.Delete;

public record DeleteClaimCommand(Guid Id) : IRequest<Response<Guid>>; //kullanılmayacak

public class DeleteClaimCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<DeleteClaimCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.AccountTransactions.GetByIdAsync(request.Id, false, cancellationToken);

        if (entity is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        entity.IsDeleted = true;
        entity.DeletedBy = Guid.Parse(currentUser.Id!);
        entity.DeletedDate = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(entity.Id);
    }
}

