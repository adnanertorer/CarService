using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Delete
{

    public record DeleteMainServiceCommand(Guid Id) : IRequest<Response<Guid>>;

    public class DeleteMainServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService, ILogger<DeleteMainServiceCommandHandler> logger)
        : IRequestHandler<DeleteMainServiceCommand, Response<Guid>>
    {
        const string redisKeyPrefix = "mainservice:list";
        const string redisSubServiceKeyPrefix = "subservice:list";
        public async Task<Response<Guid>> Handle(DeleteMainServiceCommand request, CancellationToken cancellationToken)
        {
            var companyId = currentUser.ValidCompanyId();

            var entity = await unitOfWork.MainServices.GetByIdAsync(request.Id, false, cancellationToken);

            if (entity is null)
                return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

            entity.IsDeleted = true;
            entity.DeletedBy = Guid.Parse(currentUser.Id!);
            entity.DeletedDate = DateTime.UtcNow;

            var subServices = await dbContext.SubServices.Where(i => i.MainServiceId == request.Id).ToListAsync(cancellationToken);
            foreach(var item in subServices)
            {
                item.IsDeleted = true;
                item.DeletedBy = Guid.Parse(currentUser.Id!);
                item.DeletedDate = DateTime.UtcNow;

                var cacheSubServiceKey = $"{redisSubServiceKeyPrefix}:{companyId}";
                await cacheService.RemoveFromListAsync<dynamic>(cacheSubServiceKey, item.Id.ToString());
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            try
            {
                var cacheListKey = $"{redisKeyPrefix}:{companyId}";
                await cacheService.RemoveFromListAsync<dynamic>(cacheListKey, request.Id.ToString());
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error while deleting to cache for main service delete.");
            }
           

            return Response<Guid>.Success(entity.Id);
        }
    }
}
