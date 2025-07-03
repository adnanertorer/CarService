using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Dtos.Auth;
using Adoroid.CarService.Application.Features.Users.Dtos;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Adoroid.CarService.Infrastructure.Auth;

public class TokenHandler : ITokenHandler
{
    private readonly TokenOptions _tokenOptions;
    private readonly CarServiceDbContext _dbContext;
    public TokenHandler(IOptions<TokenOptions> tokenOptions, CarServiceDbContext dbContext)
    {
        _tokenOptions = tokenOptions.Value;
        _dbContext = dbContext;
    }

    public async Task<Response<AccesTokenDto>> CreateAccessToken(UserDto user, CancellationToken cancellationToken)
    {
        var userToCompany = await _dbContext.UserToCompanies
           .AsNoTracking()
           .FirstOrDefaultAsync(i => i.UserId == user.Id, cancellationToken);

        if (userToCompany is not null)
            user.CompanyId = userToCompany.CompanyId;

        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);
        var securityKey = SignHandler.GetSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer, audience: _tokenOptions.Audience, expires: accessTokenExpiration,
            notBefore: DateTime.UtcNow, signingCredentials: signingCredentials, claims: GetClaims(user));
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);
        var accessToken = new AccesTokenDto
        {
            Token = token,
            RefreshToken = CreateRefreshToken(),
            Expiration = accessTokenExpiration,
            FullName = user.Name + " " + user.Surname,
            UserId = user.Id,
            CompanyId = user.CompanyId
        };
        user.RefreshToken = accessToken.RefreshToken;
        user.RefreshTokenEndDate = accessToken.Expiration;

        var userEntity = await _dbContext.Users.FirstOrDefaultAsync(i => i.Id == user.Id, cancellationToken);
        if(userEntity is null)
            return Response<AccesTokenDto>.Fail("User not found");

        userEntity.RefreshToken = accessToken.RefreshToken;
        userEntity.RefreshTokenExpr = accessToken.Expiration;

        _dbContext.Entry(userEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _dbContext.Users.Update(userEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Response<AccesTokenDto>.Success(accessToken, "Access token created successfully");
    }

    private static string CreateRefreshToken()
    {
        var numberByte = new byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }

    public Response<AccesTokenDto> ReturnAccessToken(UserDto user)
    {
        if (user.RefreshTokenEndDate == null || user.RefreshToken is null)
        {
            return Response<AccesTokenDto>.Fail("Refresh token end date or token is null");
        }

        var accessTokenExpiration = user.RefreshTokenEndDate.Value;
        var securityKey = SignHandler.GetSecurityKey(_tokenOptions.SecurityKey);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer, audience: _tokenOptions.Audience, expires: accessTokenExpiration,
            notBefore: DateTime.UtcNow, signingCredentials: signingCredentials, claims: GetClaims(user));

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);

        var accessToken = new AccesTokenDto
        {
            Token = token,
            RefreshToken = user.RefreshToken!,
            Expiration = accessTokenExpiration,
            FullName = user.Name + " " + user.Surname,
            UserId = user.Id,
            CompanyId = user.CompanyId
        };
        return Response<AccesTokenDto>.Success(accessToken);
    }

    private static List<Claim> GetClaims(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, $"{ user.Name} { user.Surname }"),
            new(type: ClaimTypes.UserData, value: user.CompanyId.HasValue ? user.CompanyId.Value.ToString() : string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("user_type", "company")
        };

        return claims;
    }

    public void RevokeRefreshToken(UserDto user)
    {
        user.RefreshToken = null;
    }
}
