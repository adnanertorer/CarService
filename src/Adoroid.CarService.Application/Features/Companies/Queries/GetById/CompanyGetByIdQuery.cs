using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using Adoroid.CarService.Application.Features.Companies.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Queries.GetById;

public record CompanyGetByIdQuery(Guid Id) : IRequest<Response<CompanyDto>>;

public class CompanyGetByIdQueryHandler(CarServiceDbContext dbContext) : IRequestHandler<CompanyGetByIdQuery, Response<CompanyDto>>
{
    public async Task<Response<CompanyDto>> Handle(CompanyGetByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies
            .Include(i => i.City)
            .Include(i => i.District)
            .Include(i => i.CompanyServices).ThenInclude(i => i.MasterService)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (company is null)
            return Response<CompanyDto>.Fail(BusinessExceptionMessages.CompanyNotFound);

        return Response<CompanyDto>.Success(company.FromEntity());
    }
}
