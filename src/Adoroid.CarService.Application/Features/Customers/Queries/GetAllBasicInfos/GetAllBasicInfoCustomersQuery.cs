using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;
using System.Linq.Dynamic.Core;

namespace Adoroid.CarService.Application.Features.Customers.Queries.GetAllBasicInfos;

public record GetAllBasicInfoCustomersQuery: IRequest<Response<List<CustomerBasicInfoDto>>>;

public class GetAllCustomersQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService) : IRequestHandler<GetAllBasicInfoCustomersQuery, Response<List<CustomerBasicInfoDto>>>
{
    public async Task<Response<List<CustomerBasicInfoDto>>> Handle(GetAllBasicInfoCustomersQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();
        var redisKeyPrefix = $"customers-basic-info:{companyId}";

        var list = await cacheService.GetOrSetListAsync<List<CustomerBasicInfoDto>>(redisKeyPrefix,
            async () =>
            {
                var query = unitOfWork.Customers.GetBasicInfoByCompanyId(companyId);

                return await query
                    .ToListAsync(cancellationToken);

            }, TimeSpan.FromHours(2));

        return Response<List<CustomerBasicInfoDto>>.Success(list);
    }
}
