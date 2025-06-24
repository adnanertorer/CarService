using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Delete
{

    public record DeleteMainServiceCommand(Guid Id) : IRequest<Response<Guid>>, ICacheRemovableCommand
    {
        public IEnumerable<string> GetCacheKeysToRemove()
        {
            yield return $"main-service:{Id}";
            yield return "main-service:list";
        }
    }

    public class DeleteMainServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
        : IRequestHandler<DeleteMainServiceCommand, Response<Guid>>
    {
        public async Task<Response<Guid>> Handle(DeleteMainServiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.MainServices.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (entity is null)
                return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

            entity.IsDeleted = true;
            entity.DeletedBy = Guid.Parse(currentUser.Id!);
            entity.DeletedDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Response<Guid>.Success(entity.Id);
        }
    }
}
