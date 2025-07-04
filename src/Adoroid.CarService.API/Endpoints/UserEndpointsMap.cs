﻿using Adoroid.CarService.API.Extensions;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.Users.Commands.Create;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Application.Features.Users.Queries.GetRefreshToken;
using Adoroid.CarService.Application.Features.Users.Queries.Login;

namespace Adoroid.CarService.API.Endpoints;

public static class UserEndpointsMap
{
    private const string apiPath = "/api/user";
    public static IEndpointRouteBuilder UserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MinimalMediatrMapCommand<UserLoginQuery, AccesTokenDto>(apiPath+"/login");
        builder.MinimalMediatrMapCommand<CreateAccessTokenByRefreshTokenQuery, AccesTokenDto>(apiPath+"/refresh");
        builder.MinimalMediatrMapCommand<CreateUserCommand, UserDto>(apiPath);
        return builder;
    }
}
