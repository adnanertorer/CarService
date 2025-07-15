using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Delete;

public record DeleteSubServiceCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteSubServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService, ILogger<DeleteSubServiceCommandHandler> logger)
    : IRequestHandler<DeleteSubServiceCommand, Response<Guid>>
{
    const string redisKeyPrefix = "subservice:list";
    public async Task<Response<Guid>> Handle(DeleteSubServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var entity = await unitOfWork.SubServices.GetByIdAsync(request.Id, false, cancellationToken);

        if (entity is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        entity.IsDeleted = true;
        entity.DeletedBy = Guid.Parse(currentUser.Id!);
        entity.DeletedDate = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            var cacheListKey = $"{redisKeyPrefix}:{companyId}";
            await cacheService.RemoveFromListAsync<dynamic>(cacheListKey, request.Id.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting to cache for sub service delete.");
        }

        return Response<Guid>.Success(entity.Id);
    }
}

