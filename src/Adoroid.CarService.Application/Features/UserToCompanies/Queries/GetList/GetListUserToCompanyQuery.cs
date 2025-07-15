using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Application.Features.UserToCompanies.MappingExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Queries.GetList;

public record GetListUserToCompanyQuery(PageRequest PageRequest)
    : IRequest<Response<Paginate<UserToCompanyDto>>>;

public class GetListUserToCompanyQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetListUserToCompanyQuery, Response<Paginate<UserToCompanyDto>>>
{
    public async Task<Response<Paginate<UserToCompanyDto>>> Handle(GetListUserToCompanyQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var query = unitOfWork.UserToCompanies.GetQueryable(companyId);

            var result = await query
                .OrderByDescending(i => i.CreatedDate)
                .Select(i => i.FromEntity())
                .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<UserToCompanyDto>>.Success(result);
    }
}

