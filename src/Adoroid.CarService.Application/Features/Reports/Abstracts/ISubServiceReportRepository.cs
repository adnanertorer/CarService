using Adoroid.CarService.Application.Features.Reports.Dtos;

namespace Adoroid.CarService.Application.Features.Reports.Abstracts;

public interface ISubServiceReportRepository
{
    Task<List<EmployeeServiceCountDto>> GetServiceCountByEmplooye(Guid companyId, CancellationToken cancellationToken = default);
}
