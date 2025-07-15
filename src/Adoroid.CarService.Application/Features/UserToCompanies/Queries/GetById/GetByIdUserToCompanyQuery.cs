using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Application.Features.UserToCompanies.ExceptionMessages;
using Adoroid.CarService.Application.Features.UserToCompanies.MappingExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Queries.GetById;

public record GetByIdUserToCompanyQuery(Guid Id) : IRequest<Response<UserToCompanyDto>>;

public class GetByIdUserToCompanyQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetByIdUserToCompanyQuery, Response<UserToCompanyDto>>
{

    public async Task<Response<UserToCompanyDto>> Handle(GetByIdUserToCompanyQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.UserToCompanies.GetByIdWithIncludedAsync(request.Id, true, cancellationToken);

        if (entity is null)
            return Response<UserToCompanyDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<UserToCompanyDto>.Success(entity.FromEntity());
    }
}

