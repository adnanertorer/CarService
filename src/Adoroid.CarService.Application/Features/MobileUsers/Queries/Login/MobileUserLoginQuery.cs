using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.MobileUsers.ExceptionMessages;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MobileUsers.Queries.Login;

public record MobileUserLoginQuery(string Email, string Password) : IRequest<Response<MobileUserAccessTokenDto>>;

public class MobileUserLoginQueryHandler(CarServiceDbContext dbContext, IMobileUserTokenHandler tokenHandler, IAesEncryptionHelper aesEncryptionHelper)
    : IRequestHandler<MobileUserLoginQuery, Response<MobileUserAccessTokenDto>{
    public async Task<Response<MobileUserAccessTokenDto>> Handle(MobileUserLoginQuery request, CancellationToken cancellationToken)
    {
        var encryptedPassword = aesEncryptionHelper.Encrypt(request.Password);
        var user = await dbContext.MobileUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Email == request.Email && i.Password == encryptedPassword, cancellationToken);

        if (user is null)
            return Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.UserNotFound);

        var accessToken = await tokenHandler.CreateAccessToken(new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
        }, cancellationToken);

        if (accessToken is not { Succeeded: true, Data: not null })
            return Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.LoginFailed);

        return Response<MobileUserAccessTokenDto>.Success(accessToken.Data);
    }
}
