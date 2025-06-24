using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Delete
{

    public record DeleteMainServiceCommand(Guid Id) : IRequest<Response<Guid>>;

    public class DeleteMainServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ICacheService cacheService)
        : IRequestHandler<DeleteMainServiceCommand, Response<Guid>>
    {
        const string redisKeyPrefix = "mainservice:list";
        public async Task<Response<Guid>> Handle(DeleteMainServiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.MainServices.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (entity is null)
                return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

            entity.IsDeleted = true;
            entity.DeletedBy = Guid.Parse(currentUser.Id!);
            entity.DeletedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            var cacheListKey = $"{redisKeyPrefix}:{currentUser.CompanyId!}";
            await cacheService.RemoveFromListAsync<dynamic>(cacheListKey, request.Id.ToString());

            return Response<Guid>.Success(entity.Id);
        }
    }
}
