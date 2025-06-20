﻿using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.Core.Application.Wrappers;

namespace Adoroid.CarService.Application.Common.Abstractions.Auth;

public interface ITokenHandler
{
    Task<Response<AccesTokenDto>> CreateAccessToken(UserDto user, CancellationToken cancellationToken);
    Response<AccesTokenDto> ReturnAccessToken(UserDto user);
    void RevokeRefreshToken(UserDto user);
}
