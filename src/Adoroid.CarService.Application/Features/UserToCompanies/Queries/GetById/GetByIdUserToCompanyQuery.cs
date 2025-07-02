using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Application.Features.UserToCompanies.ExceptionMessages;
using Adoroid.CarService.Application.Features.UserToCompanies.MappingExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Queries.GetById;

public record GetByIdUserToCompanyQuery(Guid Id) : IRequest<Response<UserToCompanyDto>>;

public class GetByIdUserToCompanyQueryHandler(CarServiceDbContext dbContext)
    : IRequestHandler<GetByIdUserToCompanyQuery, Response<UserToCompanyDto>>
{

    public async Task<Response<UserToCompanyDto>> Handle(GetByIdUserToCompanyQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.UserToCompanies
            .Include(e => e.Company)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<UserToCompanyDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<UserToCompanyDto>.Success(entity.FromEntity());
    }
}

