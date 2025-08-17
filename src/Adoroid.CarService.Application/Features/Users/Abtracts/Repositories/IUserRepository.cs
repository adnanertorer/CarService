using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Users.Abtracts.Repositories;

public interface IUserRepository
{
    Task<User?> Create(string name, string surname, string email, string password, string phoneNumber, CancellationToken cancellationToken = default);
    Task<User?> GetUserWithEmailAndPassword(string email, string password, CancellationToken cancellationToken = default);

    Task<bool> AnyUserWithEmailAndPhonenumber(string email, string phoneNumber, CancellationToken cancellationToken = default);
    Task<AccesTokenDto> AccessTokenByRefreshToken(string refreshToken, CancellationToken cancellationToken = default);
    Task<User?> GetUserByRefreshToken(string refreshToken, CancellationToken cancellationToken = default);
    Task<UserToCompany?> GetUserToCompany(Guid userId, CancellationToken cancellationToken = default);
    void Update(User user, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserByOtpCodeAsync(string otpCode, CancellationToken cancellationToken = default);
    Task<User?> GetUserWithEmailAddress(string email, CancellationToken cancellationToken = default);
}
