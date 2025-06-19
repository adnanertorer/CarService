using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetById;

public record GetByIdSubServiceQuery(Guid Id) : IRequest<Response<SubServiceDto>>;

public class GetEntityByIdQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetByIdSubServiceQuery, Response<SubServiceDto>>
{

    public async Task<Response<SubServiceDto>> Handle(GetByIdSubServiceQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.SubServices
            .AsNoTracking()
            .Include(i => i.MainService).ThenInclude(i => i.Vehicle)
            .Include(i => i.Employee)
            .Include(i => i.Supplier)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<SubServiceDto>.Success(entity.FromEntity());
    }
}

