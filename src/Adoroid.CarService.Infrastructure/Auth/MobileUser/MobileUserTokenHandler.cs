using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.MobileUsers.Dtos;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Adoroid.CarService.Infrastructure.Auth.MobileUser;

public class MobileUserTokenHandler : IMobileUserTokenHandler
{
    private readonly MobileTokenOptions _tokenOptions;
    private readonly CarServiceDbContext _dbContext;
     public MobileUserTokenHandler(IOptions<MobileTokenOptions> tokenOptions, CarServiceDbContext dbContext)
    {
        _tokenOptions = tokenOptions.Value;
        _dbContext = dbContext;
    }

    public async Task<Response<MobileUserAccessTokenDto>> CreateAccessToken(MobileUserDto user, CancellationToken cancellationToken)
    {
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);
        var securityKey = SignHandler.GetSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer, audience: _tokenOptions.Audience, expires: accessTokenExpiration,
            notBefore: DateTime.UtcNow, signingCredentials: signingCredentials, claims: GetClaims(user));
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);
        var accessToken = new MobileUserAccessTokenDto
        {
            Token = token,
            RefreshToken = CreateRefreshToken(),
            Expiration = accessTokenExpiration,
            FullName = user.Name + " " + user.Surname,
            UserId = user.Id
        };
        user.RefreshToken = accessToken.RefreshToken;
        user.RefreshTokenEndDate = accessToken.Expiration;

        var userEntity = await _dbContext.MobileUsers.FirstOrDefaultAsync(i => i.Id == user.Id, cancellationToken);
        if(userEntity is null)
            return Response<MobileUserAccessTokenDto>.Fail("User not found");

        userEntity.RefreshToken = accessToken.RefreshToken;
        userEntity.RefreshTokenExpr = accessToken.Expiration;

        _dbContext.Entry(userEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _dbContext.MobileUsers.Update(userEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Response<MobileUserAccessTokenDto>.Success(accessToken, "Access token created successfully");
    }

    private static string CreateRefreshToken()
    {
        var numberByte = new byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }

    public Response<MobileUserAccessTokenDto> ReturnAccessToken(MobileUserDto user)
    {
        if (user.RefreshTokenEndDate == null || user.RefreshToken is null)
        {
            return Response<MobileUserAccessTokenDto>.Fail("Refresh token end date or token is null");
        }

        var accessTokenExpiration = user.RefreshTokenEndDate.Value;
        var securityKey = SignHandler.GetSecurityKey(_tokenOptions.SecurityKey);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer, audience: _tokenOptions.Audience, expires: accessTokenExpiration,
            notBefore: DateTime.UtcNow, signingCredentials: signingCredentials, claims: GetClaims(user));

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);

        var accessToken = new MobileUserAccessTokenDto
        {
            Token = token,
            RefreshToken = user.RefreshToken!,
            Expiration = accessTokenExpiration,
            FullName = user.Name + " " + user.Surname,
            UserId = user.Id
        };
        return Response<MobileUserAccessTokenDto>.Success(accessToken);
    }

    private static List<Claim> GetClaims(MobileUserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, $"{ user.Name} { user.Surname }"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("user_type", "mobileUser")
        };

        return claims;
    }

    public void RevokeRefreshToken(MobileUserDto user)
    {
        user.RefreshToken = null;
    }
}