using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.MobileUsers.Dtos;
using Adoroid.CarService.Application.Features.MobileUsers.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MobileUsers.Queries.GetRefreshToken;

public record GetMobileUserAccessTokenByRefreshToken(string RefreshToken) : IRequest<Response<MobileUserAccessTokenDto>>;

public class GetMobileUserAccessTokenByRefreshTokenHandler(CarServiceDbContext dbContext, IMobileUserTokenHandler tokenHandler) 
    : IRequestHandler<GetMobileUserAccessTokenByRefreshToken, Response<MobileUserAccessTokenDto>>
{
    public async Task<Response<MobileUserAccessTokenDto>> Handle(GetMobileUserAccessTokenByRefreshToken request, CancellationToken cancellationToken)
    {
        var user = await dbContext.MobileUsers
             .FirstOrDefaultAsync(i => i.RefreshToken == request.RefreshToken, cancellationToken);

        if (user is null)
            return Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);

        if (user.RefreshTokenExpr.HasValue && user.RefreshTokenExpr.Value < DateTime.UtcNow)
        {
            user.RefreshTokenExpr = null;
            user.RefreshToken = null;
            dbContext.MobileUsers.Update(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.RefreshTokenExpired);
        }

        if (!user.RefreshTokenExpr.HasValue)
            return Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);

        var accessTokenResponse = tokenHandler.ReturnAccessToken(new MobileUserDto
        {
            RefreshToken = request.RefreshToken,
            RefreshTokenEndDate = user.RefreshTokenExpr.Value
        });

        return accessTokenResponse is { Succeeded: true, Data: not null }
             ? Response<MobileUserAccessTokenDto>.Success(accessTokenResponse.Data)
             : Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);
    }
}
