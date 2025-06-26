using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.Core.Application.Wrappers;

namespace Adoroid.CarService.Application.Common.Abstractions.Auth;

public interface IMobileUserTokenHandler
{
    Task<Response<MobileUserAccessTokenDto>> CreateAccessToken(UserDto user, CancellationToken cancellationToken);
    Response<MobileUserAccessTokenDto> ReturnAccessToken(UserDto user);
    void RevokeRefreshToken(UserDto user);
}
