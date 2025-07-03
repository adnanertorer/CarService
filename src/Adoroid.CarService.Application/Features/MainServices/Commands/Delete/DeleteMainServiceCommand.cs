using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.BusinessMessages;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.Users.Queries.CheckCompanyId;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Delete
{

    public record DeleteMainServiceCommand(Guid Id) : IRequest<Response<Guid>>;

    public class DeleteMainServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ICacheService cacheService,
        IMediator mediator)
        : IRequestHandler<DeleteMainServiceCommand, Response<Guid>>
    {
        const string redisKeyPrefix = "mainservice:list";
        const string redisSubServiceKeyPrefix = "subservice:list";
        public async Task<Response<Guid>> Handle(DeleteMainServiceCommand request, CancellationToken cancellationToken)
        {
            var companyIdResponse = await mediator.Send(new GetCompanyIdCommand(), cancellationToken);

            if (!companyIdResponse.Succeeded)
                return Response<Guid>.Fail(BusinessMessages.CompanyNotFound);

            var companyId = companyIdResponse.Data!.Value;

            var entity = await dbContext.MainServices.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (entity is null)
                return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

            entity.IsDeleted = true;
            entity.DeletedBy = Guid.Parse(currentUser.Id!);
            entity.DeletedDate = DateTime.UtcNow;

            dbContext.MainServices.Update(entity);

            var subServices = await dbContext.SubServices.Where(i => i.MainServiceId == request.Id).ToListAsync(cancellationToken);
            foreach(var item in subServices)
            {
                item.IsDeleted = true;
                item.DeletedBy = Guid.Parse(currentUser.Id!);
                item.DeletedDate = DateTime.UtcNow;

                dbContext.SubServices.Update(item);
                var cacheSubServiceKey = $"{redisSubServiceKeyPrefix}:{companyId}";
                await cacheService.RemoveFromListAsync<dynamic>(cacheSubServiceKey, item.Id.ToString());
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            var cacheListKey = $"{redisKeyPrefix}:{companyId}";
            await cacheService.RemoveFromListAsync<dynamic>(cacheListKey, request.Id.ToString());

            return Response<Guid>.Success(entity.Id);
        }
    }
}
