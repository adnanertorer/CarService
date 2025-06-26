using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.MobileUsers.Dtos;
using Adoroid.Core.Application.Wrappers;

namespace Adoroid.CarService.Application.Common.Abstractions.Auth;

public interface IMobileUserTokenHandler
{
    Task<Response<MobileUserAccessTokenDto>> CreateAccessToken(MobileUserDto user, CancellationToken cancellationToken);
    Response<MobileUserAccessTokenDto> ReturnAccessToken(MobileUserDto user);
    void RevokeRefreshToken(MobileUserDto user);
}
