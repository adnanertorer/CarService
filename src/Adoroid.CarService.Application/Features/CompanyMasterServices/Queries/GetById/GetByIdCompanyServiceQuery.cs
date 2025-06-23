using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Application.Features.CompanyMasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.CompanyMasterServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Queries.GetById;

public record GetByIdCompanyServiceQuery(Guid Id) : IRequest<Response<CompanyServiceDto>>;

public class GetByIdCompanyServiceQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetByIdCompanyServiceQuery, Response<CompanyServiceDto>>
{
    public async Task<Response<CompanyServiceDto>> Handle(GetByIdCompanyServiceQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.CompanyServices
            .Include(i => i.MasterService)
            .Include(i => i.Company)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<CompanyServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<CompanyServiceDto>.Success(entity.FromEntity());
    }
}

