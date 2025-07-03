using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.BusinessMessages;
using Adoroid.CarService.Application.Features.Users.Queries.CheckCompanyId;
using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Application.Features.UserToCompanies.MappingExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Queries.GetList;

public record GetListUserToCompanyQuery(PageRequest PageRequest)
    : IRequest<Response<Paginate<UserToCompanyDto>>>;

public class GetListUserToCompanyQueryHandler(CarServiceDbContext dbContext, IMediator mediator)
    : IRequestHandler<GetListUserToCompanyQuery, Response<Paginate<UserToCompanyDto>>>
{
    public async Task<Response<Paginate<UserToCompanyDto>>> Handle(GetListUserToCompanyQuery request, CancellationToken cancellationToken)
    {
        var companyIdResponse = await mediator.Send(new GetCompanyIdCommand(), cancellationToken);

        if (!companyIdResponse.Succeeded)
            return Response<Paginate<UserToCompanyDto>>.Fail(BusinessMessages.CompanyNotFound);

        var companyId = companyIdResponse.Data!.Value;

        var query = dbContext.UserToCompanies
            .Include(i => i.Company)
            .Include(i => i.User)
            .AsNoTracking()
            .Where(i => i.CompanyId == companyId);

            var result = await query
                .OrderByDescending(i => i.CreatedDate)
                .Select(i => i.FromEntity())
                .ToPaginateAsync(request.PageRequest.PageIndex, request.PageRequest.PageSize, cancellationToken);

        return Response<Paginate<UserToCompanyDto>>.Success(result);
    }
}

