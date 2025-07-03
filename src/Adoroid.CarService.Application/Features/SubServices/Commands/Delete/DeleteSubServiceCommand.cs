using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Delete;

public record DeleteSubServiceCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteSubServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ICacheService cacheService)
    : IRequestHandler<DeleteSubServiceCommand, Response<Guid>>
{
    const string redisKeyPrefix = "subservice:list";
    public async Task<Response<Guid>> Handle(DeleteSubServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var entity = await dbContext.SubServices.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        entity.IsDeleted = true;
        entity.DeletedBy = Guid.Parse(currentUser.Id!);
        entity.DeletedDate = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        var cacheListKey = $"{redisKeyPrefix}:{companyId}";
        await cacheService.RemoveFromListAsync<dynamic>(cacheListKey, request.Id.ToString());

        return Response<Guid>.Success(entity.Id);
    }
}

