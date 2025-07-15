using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MasterServices.Dtos;
using Adoroid.CarService.Application.Features.MasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MasterServices.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MasterServices.Commands.Update;

public record UpdateMasterServiceCommand(Guid Id, string ServiceName, int OrderIndex) : IRequest<Response<MasterServiceDto>>;

public class UpdateMasterServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService, ILogger<UpdateMasterServiceCommandHandler> logger)
    : IRequestHandler<UpdateMasterServiceCommand, Response<MasterServiceDto>>
{
    const string redisKeyPrefix = "mainservice:list";
    public async Task<Response<MasterServiceDto>> Handle(UpdateMasterServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.MasterServices.GetByIdAsync(request.Id, asNoTracking: false, cancellationToken);

        if (entity is null)
            return Response<MasterServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        var serviceList = await unitOfWork.MasterServices.GetAllAsync(cancellationToken);

        var isExistIndex = serviceList.Any(i => i.OrderIndex == request.OrderIndex);

        if (isExistIndex)
            return Response<MasterServiceDto>.Fail(BusinessExceptionMessages.IndexAlreadyExists);

        entity.ServiceName = request.ServiceName;
        entity.OrderIndex = request.OrderIndex;
        entity.UpdatedBy = Guid.Parse(currentUser.Id!);
        entity.UpdatedDate = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            await cacheService.UpdateToListAsync(redisKeyPrefix, request.Id.ToString(), entity.FromEntity(), null);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Cache güncelleme işlemi başarısız oldu. MasterService: {MasterServiceId}";
            logger.LogError(ex, errorMessage, request.Id);
        }

        return Response<MasterServiceDto>.Success(entity.FromEntity());
    }
}

