using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetById;

public record GetByIdSubServiceQuery(Guid Id) : IRequest<Response<SubServiceDto>>;

public class GetEntityByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetByIdSubServiceQuery, Response<SubServiceDto>>
{
    public async Task<Response<SubServiceDto>> Handle(GetByIdSubServiceQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.SubServices.GetDetailByIdAsync(request.Id, cancellationToken, true);

        if (entity is null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<SubServiceDto>.Success(entity.FromEntity());
    }
}

