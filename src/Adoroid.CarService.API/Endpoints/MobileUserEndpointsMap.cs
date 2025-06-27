using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.MobileUsers.Commands.Create;
using Adoroid.CarService.Application.Features.MobileUsers.Dtos;
using Adoroid.CarService.Application.Features.MobileUsers.Queries.GetRefreshToken;
using Adoroid.CarService.Application.Features.MobileUsers.Queries.Login;

namespace Adoroid.CarService.API.Endpoints;

public static class MobileUserEndpointsMap
{
    private const string apiPath = "/api/mobileuser";
    public static IEndpointRouteBuilder MobileUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<MobileUserLoginQuery, MobileUserAccessTokenDto>(apiPath + "/login");
        builder.MinimalMediatrMapCommand<GetMobileUserAccessTokenByRefreshToken, MobileUserAccessTokenDto>(apiPath + "/refresh");
        builder.MinimalMediatrMapCommand<CreateMobilUserCommand, MobileUserDto>(apiPath);
        return builder;
    }
}
