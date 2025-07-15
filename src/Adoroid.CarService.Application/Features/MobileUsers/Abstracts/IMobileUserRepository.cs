using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.MobileUsers.Abstracts;

public interface IMobileUserRepository
{
    Task<string> GetNameById(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsExistByEmail(string email, CancellationToken cancellationToken = default);
    Task<MobileUser> AddAsync(MobileUser user, CancellationToken cancellationToken = default);
    Task<MobileUser?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<MobileUser?> GetByRefreshTokenAsync(string refreshToken, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<MobileUser?> GetByUsernamePasswordAsync(string username, string password, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, string>> GetUserNames(List<Guid> guids, CancellationToken cancellationToken);
}
