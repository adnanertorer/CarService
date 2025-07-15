using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Application.Features.CompanyMasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.CompanyMasterServices.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Queries.GetById;

public record GetByIdCompanyServiceQuery(Guid Id) : IRequest<Response<CompanyServiceDto>>;

public class GetByIdCompanyServiceQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetByIdCompanyServiceQuery, Response<CompanyServiceDto>>
{
    public async Task<Response<CompanyServiceDto>> Handle(GetByIdCompanyServiceQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.CompanyServices.GetByIdWithSubTables(request.Id, asNoTracking: true, cancellationToken);

        if (entity is null)
            return Response<CompanyServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<CompanyServiceDto>.Success(entity.FromEntity());
    }
}

