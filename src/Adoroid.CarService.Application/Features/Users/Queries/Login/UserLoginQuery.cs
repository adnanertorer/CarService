using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Dtos.Auth;
using Adoroid.CarService.Application.Dtos.Users;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Queries.Login;

public record UserLoginQuery(string Email, string Password) : IRequest<Response<AccesTokenDto>>;

public class UserLoginQueryHandler(CarServiceDbContext dbContext, ITokenHandler tokenHandler, IAesEncryptionHelper aesEncryptionHelper) : IRequestHandler<UserLoginQuery, Response<AccesTokenDto>>
{
    public async Task<Response<AccesTokenDto>> Handle(UserLoginQuery request, CancellationToken cancellationToken)
    {
        var encryptedPassword = aesEncryptionHelper.Encrypt(request.Password);
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Email == request.Email && i.Password == encryptedPassword, cancellationToken);

        if (user is null)
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.InvalidCredentials);

        var accessToken = await tokenHandler.CreateAccessToken(new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            CompanyId = user.CompanyId
        }, cancellationToken);

        if (accessToken is not { Succeeded: true, Data: not null})
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.LoginFailed);

        return Response<AccesTokenDto>.Success(accessToken.Data);
    }
}


