using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.MobileUsers.Dtos;
using Adoroid.CarService.Application.Features.MobileUsers.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MobileUsers.Queries.GetRefreshToken;

public record GetMobileUserAccessTokenByRefreshToken(string RefreshToken) : IRequest<Response<MobileUserAccessTokenDto>>;

public class GetMobileUserAccessTokenByRefreshTokenHandler(IUnitOfWork unitOfWork, IMobileUserTokenHandler tokenHandler) 
    : IRequestHandler<GetMobileUserAccessTokenByRefreshToken, Response<MobileUserAccessTokenDto>>
{
    public async Task<Response<MobileUserAccessTokenDto>> Handle(GetMobileUserAccessTokenByRefreshToken request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.MobileUsers.GetByRefreshTokenAsync(request.RefreshToken, asNoTracking: false, cancellationToken: cancellationToken);

        if (user is null)
            return Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);

        if (user.RefreshTokenExpr.HasValue && user.RefreshTokenExpr.Value < DateTime.UtcNow)
        {
            user.RefreshTokenExpr = null;
            user.RefreshToken = null;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.RefreshTokenExpired);
        }

        if (!user.RefreshTokenExpr.HasValue)
            return Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);

        var accessTokenResponse = tokenHandler.ReturnAccessToken(new MobileUserDto
        {
            RefreshToken = request.RefreshToken,
            RefreshTokenEndDate = user.RefreshTokenExpr.Value,
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname
        });

        return accessTokenResponse is { Succeeded: true, Data: not null }
             ? Response<MobileUserAccessTokenDto>.Success(accessTokenResponse.Data)
             : Response<MobileUserAccessTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);
    }
}
