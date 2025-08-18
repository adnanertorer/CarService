using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Reports.Abstracts;
using Adoroid.CarService.Application.Features.Reports.Dtos;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Reports.Queries.GetServiceCountByEmployee;

public record GetServiceCountByEmployeeQuery : IRequest<Response<List<EmployeeServiceCountDto>>>;

public class GetServiceCountByEmployeeQueryHandler(ISubServiceReportRepository subServiceReportRepository, ICurrentUser currentUser) : IRequestHandler<GetServiceCountByEmployeeQuery, Response<List<EmployeeServiceCountDto>>>
{
    public async Task<Response<List<EmployeeServiceCountDto>>> Handle(GetServiceCountByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var serviceCounts = await subServiceReportRepository.GetServiceCountByEmplooye(companyId, cancellationToken);

        return Response<List<EmployeeServiceCountDto>>.Success(serviceCounts);
    }
}
