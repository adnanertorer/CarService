﻿using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MasterServices.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MasterServices.Commands.Delete;

public record DeleteMasterServiceCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteMasterServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService, ILogger<DeleteMasterServiceCommandHandler> logger)
    : IRequestHandler<DeleteMasterServiceCommand, Response<Guid>>
{
    const string redisKeyPrefix = "mainservice:list";
    public async Task<Response<Guid>> Handle(DeleteMasterServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.MasterServices.GetByIdAsync(request.Id, asNoTracking: false, cancellationToken);

        if (entity is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        entity.IsDeleted = true;
        entity.DeletedBy = Guid.Parse(currentUser.Id!);
        entity.DeletedDate = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            await cacheService.RemoveFromListAsync<dynamic>(redisKeyPrefix, request.Id.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting to cache for master service delete.");
        }

        return Response<Guid>.Success(entity.Id);
    }
}

