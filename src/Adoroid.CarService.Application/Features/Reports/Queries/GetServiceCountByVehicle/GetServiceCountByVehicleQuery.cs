using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Reports.Abstracts;
using Adoroid.CarService.Application.Features.Reports.Dtos;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Reports.Queries.GetServiceCountByVehicle;

public record GetServiceCountByVehicleQuery : IRequest<Response<List<VehicleServiceCountDto>>>;

public class GetServiceCountByVehicleQueryHandler(ISubServiceReportRepository repository, ICurrentUser currentUser) : IRequestHandler<GetServiceCountByVehicleQuery, Response<List<VehicleServiceCountDto>>>
{
    public async Task<Response<List<VehicleServiceCountDto>>> Handle(GetServiceCountByVehicleQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();
        var serviceCounts = await repository.GetServiceCountByVehicles(companyId, cancellationToken);
        return Response<List<VehicleServiceCountDto>>.Success(serviceCounts);
    }
}
