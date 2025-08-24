using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.Core.Application.Requests;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Queries.GetList;

public record GetListMySubServiceQuery(PageRequest PageRequest, Guid MainServiceId) : IRequest<Response<Paginate<MobileSubServiceDto>>>;

public class GetListMySubServiceQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetListMySubServiceQuery, Response<Paginate<MobileSubServiceDto>>>
{
    private const string MobileUser = "mobileUser";
    public async Task<Response<Paginate<MobileSubServiceDto>>> Handle(GetListMySubServiceQuery request, CancellationToken cancellationToken)
    {

        if(currentUser.UserType != MobileUser)
            return Response<Paginate<MobileSubServiceDto>>.Fail(BusinessExceptionMessages.YouAreNotAuthorized);

        var query = unitOfWork.SubServices.GetByMainServiceIdAndUser(request.MainServiceId, Guid.Parse(currentUser.Id!), true);

        var list = await query
            .OrderByDescending(i => i.OperationDate)
            .Select(i => i.FromEntityToMobile()).ToListAsync(cancellationToken);

        return Response<Paginate<MobileSubServiceDto>>.Success(list.AsQueryable().ToPaginate(request.PageRequest.PageIndex, request.PageRequest.PageSize));
    }
}

