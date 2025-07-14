using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Queries.Login;

public record UserLoginQuery(string Email, string Password) : IRequest<Response<AccesTokenDto>>;

public class UserLoginQueryHandler(IUnitOfWork unitOfWork, ITokenHandler tokenHandler, IAesEncryptionHelper aesEncryptionHelper) : IRequestHandler<UserLoginQuery, Response<AccesTokenDto>>
{
    public async Task<Response<AccesTokenDto>> Handle(UserLoginQuery request, CancellationToken cancellationToken)
    {
        var encryptedPassword = aesEncryptionHelper.Encrypt(request.Password);
        var user = await unitOfWork.Users
            .GetUserWithEmailAndPassword(request.Email, encryptedPassword, cancellationToken);

        if (user is null)
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.InvalidCredentials);

        var accessToken = await tokenHandler.CreateAccessToken(new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
        }, cancellationToken);

        if (accessToken is not { Succeeded: true, Data: not null})
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.LoginFailed);

        return Response<AccesTokenDto>.Success(accessToken.Data);
    }
}


