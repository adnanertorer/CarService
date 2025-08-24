using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Delete
{
    public record DeleteMainServiceCommand(Guid Id) : IRequest<Response<Guid>>;

    public class DeleteMainServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ILogger<DeleteMainServiceCommandHandler> logger)
        : IRequestHandler<DeleteMainServiceCommand, Response<Guid>>
    {
        public async Task<Response<Guid>> Handle(DeleteMainServiceCommand request, CancellationToken cancellationToken)
        {
            var companyId = currentUser.ValidCompanyId();

            var entity = await unitOfWork.MainServices.GetByIdAsync(request.Id, false, cancellationToken);

            if (entity is null)
                return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

            entity.IsDeleted = true;
            entity.DeletedBy = Guid.Parse(currentUser.Id!);
            entity.DeletedDate = DateTime.UtcNow;

            var subServices = await unitOfWork.SubServices.GetListByMainServiceIdAsync(request.Id, false, cancellationToken);
            foreach(var item in subServices)
            {
                item.IsDeleted = true;
                item.DeletedBy = Guid.Parse(currentUser.Id!);
                item.DeletedDate = DateTime.UtcNow;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Main service with id {Id} deleted by user {UserId}", entity.Id, currentUser.Id);

            return Response<Guid>.Success(entity.Id);
        }
    }
}
