using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Cities.Abstracts;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken = default);
}
