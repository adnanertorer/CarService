using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.Users.Abtracts.Repositories;
using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Persistence.Repositories;

public class UserRepository(CarServiceDbContext dbContext) : IUserRepository
{
    
    public async Task<AccesTokenDto> AccessTokenByRefreshToken(string refreshToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> Create(string name, string surname, string email, string password, string phoneNumber, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByRefreshToken(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(i => i.RefreshToken == refreshToken, cancellationToken);
        return user;
    }

    public async Task<UserToCompany?> GetUserToCompany(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserToCompanies
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.UserId == userId, cancellationToken);
    }

    public async Task<User?> GetUserWithEmailAndPassword(string email, string password, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Email == email && i.Password == password, cancellationToken);
    }

    public async Task<bool> AnyUserWithEmailAndPhonenumber(string email, string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AsNoTracking()
            .AnyAsync(i => i.Email == email || i.PhoneNumber == phoneNumber, cancellationToken);
    }

    public void Update(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Update(user);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
       await dbContext.Users.AddAsync(user, cancellationToken);
    }
}
