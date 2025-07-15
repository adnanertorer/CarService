using Adoroid.CarService.Application.Features.MobileUsers.Abstracts;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adoroid.CarService.Persistence.Repositories;

public class MobileUserRepository(CarServiceDbContext dbContext) : IMobileUserRepository
{
    public async Task<MobileUser> AddAsync(MobileUser user, CancellationToken cancellationToken = default)
    {
        await dbContext.MobileUsers.AddAsync(user, cancellationToken);
        return user;
    }

    public async Task<MobileUser?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
            dbContext.MobileUsers.AsNoTracking() :
            dbContext.MobileUsers;

        return await query.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<MobileUser?> GetByRefreshTokenAsync(string refreshToken, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
            dbContext.MobileUsers.AsNoTracking() :
            dbContext.MobileUsers;

        return await query.FirstOrDefaultAsync(i => i.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task<MobileUser?> GetByUsernamePasswordAsync(string username, string password, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = asNoTracking ?
            dbContext.MobileUsers.AsNoTracking() :
            dbContext.MobileUsers;

        return await query.FirstOrDefaultAsync(i => i.Email == username && i.Password == password, cancellationToken);
    }

    public async Task<string> GetNameById(Guid id, CancellationToken cancellationToken = default)
    {
       var user = await dbContext.MobileUsers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new { x.Name, x.Surname })
            .FirstOrDefaultAsync(cancellationToken);

        return user is not null ? $"{user.Name} {user.Surname}" : string.Empty;
    }

    public async Task<Dictionary<Guid, string>> GetUserNames(List<Guid> guids, CancellationToken cancellationToken)
    {
        return await dbContext.MobileUsers
            .AsNoTracking()
            .Where(m => guids.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id, m => $"{m.Name} {m.Surname}", cancellationToken);
    }

    public async Task<bool> IsExistByEmail(string email, CancellationToken cancellationToken = default)
    {
       return await dbContext.MobileUsers
            .AsNoTracking()
            .Where(i => i.Email == email)
            .AnyAsync(cancellationToken);
    }
}
