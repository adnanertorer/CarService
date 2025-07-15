using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Queries.GetRefreshToken;

public record CreateAccessTokenByRefreshTokenQuery(string RefreshToken) : IRequest<Response<AccesTokenDto>>;

public class CreateAccessTokenByRefreshTokenQueryHandler(IUnitOfWork unitOfWork, ITokenHandler tokenHandler) : IRequestHandler<CreateAccessTokenByRefreshTokenQuery, Response<AccesTokenDto>>
{
    public async Task<Response<AccesTokenDto>> Handle(CreateAccessTokenByRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetUserByRefreshToken(request.RefreshToken, cancellationToken);

        if (user is null)
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);

        var userToCompany = await unitOfWork.Users.GetUserToCompany(user.Id, cancellationToken);

        if (userToCompany is null)
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.CompanyNotFound);

        if (user.RefreshTokenExpr.HasValue && user.RefreshTokenExpr.Value < DateTime.UtcNow)
        {
            user.RefreshTokenExpr = null;
            user.RefreshToken = null; 
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.RefreshTokenExpired);
        }

        if(!user.RefreshTokenExpr.HasValue)
            return Response<AccesTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);

        var accessTokenResponse = tokenHandler.ReturnAccessToken(new UserDto
        {
            RefreshToken = request.RefreshToken,
            RefreshTokenEndDate = user.RefreshTokenExpr.Value,
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            CompanyId = userToCompany.CompanyId
        });

        return accessTokenResponse is { Succeeded: true, Data: not null }
             ? Response<AccesTokenDto>.Success(accessTokenResponse.Data)
             : Response<AccesTokenDto>.Fail(BusinessExceptionMessages.InvalidRefreshToken);
    }
}   
