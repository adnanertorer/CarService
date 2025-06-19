using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Application.Features.MainServices.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Queries.GetById;

public record GetByIdMainServiceQuery(Guid Id) : IRequest<Response<MainServiceDto>>;

public class GetEntityByIdMainServiceQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetByIdMainServiceQuery, Response<MainServiceDto>>
{

    public async Task<Response<MainServiceDto>> Handle(GetByIdMainServiceQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.MainServices
            .Include(i => i.Vehicle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<MainServiceDto>.Success(entity.FromEntity());
    }
}


