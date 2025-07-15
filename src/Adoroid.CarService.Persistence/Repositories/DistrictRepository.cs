using Adoroid.CarService.Application.Features.Districts.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class DistrictRepository(CarServiceDbContext dbContext) : IDistrictRepository
{
    public async Task<IEnumerable<District>> GetDistricts(int cityId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Districts
            .AsNoTracking()
            .Where(d => d.CityId == cityId)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);
    }
}
