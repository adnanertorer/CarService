using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Queries.GetRefreshToken;

public record CreateAccessTokenByRefreshTokenQuery(string RefreshToken) : IRequest<Response<AccesTokenDto>>;

public class CreateAccessTokenByRefreshTokenQueryHandler(CarServiceDbContext dbContext, ITokenHandler tokenHandler) : IRequestHandler<CreateAccessTokenByRefreshTokenQuery, Response<AccesTokenDto>>
{
    public async Task<Response<AccesTokenDto>> Handle(CreateAccessTokenByRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(i => i.RefreshToken == request.RefreshToken, cancellationToken);

        if (user is null)
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);

        if(user.RefreshTokenExpr.HasValue && user.RefreshTokenExpr.Value < DateTime.UtcNow)
        {
            user.RefreshTokenExpr = null;
            user.RefreshToken = null; 
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.RefreshTokenExpired);
        }

        if(!user.RefreshTokenExpr.HasValue)
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);

        var accessTokenResponse = tokenHandler.ReturnAccessToken(new UserDto
        {
            RefreshToken = request.RefreshToken,
            RefreshTokenEndDate = user.RefreshTokenExpr.Value
        });

        return accessTokenResponse is { Succeeded: true, Data: not null }
             ? Response<AccesTokenDto>.Success(accessTokenResponse.Data)
             : Response<AccesTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);
    }
}   
