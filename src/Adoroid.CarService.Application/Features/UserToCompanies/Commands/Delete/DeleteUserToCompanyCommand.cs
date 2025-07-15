using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.UserToCompanies.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Commands.Delete;

public record DeleteUserToCompanyCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteUserToCompanyCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<DeleteUserToCompanyCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteUserToCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.UserToCompanies.GetByIdAsync(request.Id, false, cancellationToken);

        if (entity is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        entity.IsDeleted = true;
        entity.DeletedBy = Guid.Parse(currentUser.Id!);
        entity.DeletedDate = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(entity.Id);
    }
}

