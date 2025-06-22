using Adoroid.CarService.Application.Features.MasterServices.Dtos;
using Adoroid.CarService.Application.Features.MasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MasterServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MasterServices.Queries.GetById;

public record GetByIdMasterServiceQuery(Guid Id) : IRequest<Response<MasterServiceDto>>;

public class GetByIdMasterServiceQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetByIdMasterServiceQuery, Response<MasterServiceDto>>
{

    public async Task<Response<MasterServiceDto>> Handle(GetByIdMasterServiceQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.MasterServices
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<MasterServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<MasterServiceDto>.Success(entity.FromEntity());
    }
}

