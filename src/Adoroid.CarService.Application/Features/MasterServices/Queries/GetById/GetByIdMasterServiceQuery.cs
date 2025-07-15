using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.MasterServices.Dtos;
using Adoroid.CarService.Application.Features.MasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MasterServices.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MasterServices.Queries.GetById;

public record GetByIdMasterServiceQuery(Guid Id) : IRequest<Response<MasterServiceDto>>;

public class GetByIdMasterServiceQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetByIdMasterServiceQuery, Response<MasterServiceDto>>
{

    public async Task<Response<MasterServiceDto>> Handle(GetByIdMasterServiceQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.MasterServices.GetByIdAsync(request.Id, asNoTracking: true, cancellationToken);

        if (entity is null)
            return Response<MasterServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<MasterServiceDto>.Success(entity.FromEntity());
    }
}

