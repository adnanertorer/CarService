using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Companies.Dtos;
using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using Adoroid.CarService.Application.Features.Companies.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Queries.GetById;

public record CompanyGetByIdQuery(Guid Id) : IRequest<Response<CompanyDto>>;

public class CompanyGetByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CompanyGetByIdQuery, Response<CompanyDto>>
{
    public async Task<Response<CompanyDto>> Handle(CompanyGetByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await unitOfWork.Companies.GetByIdAsNoTrackingAsync(request.Id, cancellationToken);

        if (company is null)
            return Response<CompanyDto>.Fail(BusinessExceptionMessages.CompanyNotFound);

        return Response<CompanyDto>.Success(company.FromEntity());
    }
}
