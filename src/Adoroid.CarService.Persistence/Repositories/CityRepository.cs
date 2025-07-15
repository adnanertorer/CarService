using Adoroid.CarService.Application.Features.Cities.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class CityRepository(CarServiceDbContext dbContext) : ICityRepository
{
    public async Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken = default)
    {
       return await dbContext.Cities
            .AsNoTracking()
            .OrderBy(i => i.Name)
            .ToListAsync(cancellationToken);
    }
}
