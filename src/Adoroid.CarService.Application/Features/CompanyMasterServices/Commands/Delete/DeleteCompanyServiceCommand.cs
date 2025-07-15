using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.CompanyMasterServices.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Delete;

public record DeleteCompanyServiceCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteCompanyServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<DeleteCompanyServiceCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteCompanyServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.CompanyServices.GetById(request.Id, false, cancellationToken);

        if (entity is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        entity.IsDeleted = true;
        entity.DeletedBy = Guid.Parse(currentUser.Id!);
        entity.DeletedDate = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(entity.Id);
    }
}

