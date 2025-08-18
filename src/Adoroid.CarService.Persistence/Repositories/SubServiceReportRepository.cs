using Adoroid.CarService.Application.Features.Reports.Abstracts;
using Adoroid.CarService.Application.Features.Reports.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class SubServiceReportRepository(CarServiceDbContext dbContext) : ISubServiceReportRepository
{
    public async Task<List<EmployeeServiceCountDto>> GetServiceCountByEmplooye(Guid companyId, CancellationToken cancellationToken = default)
    {
        var serviceCounts = await dbContext.SubServices
            .AsNoTracking()
            .Include(i => i.MainService)
            .Include(i => i.Employee)
            .Where(s => s.MainService != null && s.MainService.CompanyId == companyId && s.Employee != null)
            .GroupBy(s => s.EmployeeId)
            .Select(g => new EmployeeServiceCountDto
            {
                EmployeeId = g.Key,
                EmployeeName = g.FirstOrDefault() != null && g.FirstOrDefault().Employee != null
                    ? $"{g.FirstOrDefault().Employee.Name} {g.FirstOrDefault().Employee.Surname}"
                    : "Unknown",
                ServiceCount = g.Count()
            })
            .ToListAsync(cancellationToken);
        return serviceCounts;
    }
}
