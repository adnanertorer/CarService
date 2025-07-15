using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Districts.Abstracts;

public interface IDistrictRepository
{
    Task<IEnumerable<District>> GetDistricts(int cityId, CancellationToken cancellationToken = default);
}
