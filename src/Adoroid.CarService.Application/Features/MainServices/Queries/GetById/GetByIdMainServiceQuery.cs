using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Queries.GetById;

public record GetByIdMainServiceQuery(Guid Id) : ICacheableQuery<Response<MainServiceDto>>, IRequest<Response<MainServiceDto>>
{
    public TimeSpan? Expiration => TimeSpan.FromHours(2);
    public string GetCacheKey() => $"mainservice:{Id}";
}

public class GetEntityByIdMainServiceQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetByIdMainServiceQuery, Response<MainServiceDto>>
{

    public async Task<Response<MainServiceDto>> Handle(GetByIdMainServiceQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.MainServices
            .Include(i => i.Vehicle).ThenInclude(i => i.VehicleUsers)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity == null || entity.Vehicle == null || entity.Vehicle.VehicleUsers == null) // Mutlaka bir kullanıcıya ait olması gerekir
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<MainServiceDto>.Success(entity.FromEntity());
    }
}


