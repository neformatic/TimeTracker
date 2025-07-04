using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TimeTracker.BLL.Models.Token;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Enums;
using TimeTracker.Common.Exceptions;
using TimeTracker.Common.Models.Settings;
using TimeTracker.DAL;
using TimeTracker.DAL.Entities;

namespace TimeTracker.BLL.Services;

public class TokenService : ITokenService
{
    private readonly AppSettings _appSettings;
    private readonly TimeTrackerDbContext _dbContext;

    public TokenService(IOptions<AppSettings> appSettings,
        TimeTrackerDbContext dbContext)
    {
        _appSettings = appSettings.Value;
        _dbContext = dbContext;
    }

    public string GenerateJwtToken(JwtTokenArgsModel jwtTokenArgsModel)
    {
        var symmetricSecurityKey = GetSymmetricSecurityKey();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = CreateClaimsIdentity(jwtTokenArgsModel),
            Expires = DateTime.UtcNow.AddMinutes(_appSettings.JwtTokenLifetimeMinutes),
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> CreateRefreshTokenAsync(long userId)
    {
        var userRefreshToken = new UserRefreshToken
        {
            UserId = userId,
            RefreshToken = GenerateRefreshToken(),
            CreatedDateTime = DateTimeOffset.UtcNow
        };

        _dbContext.UserRefreshTokens.Add(userRefreshToken);
        await _dbContext.SaveChangesAsync();

        return userRefreshToken.RefreshToken;
    }

    public async Task<TokensModel> RefreshTokensAsync(string expiredJwtToken, string refreshToken)
    {
        var userId = ValidateExpiredJwtToken(expiredJwtToken);
        if (!userId.HasValue)
        {
            throw new BadRequestException(ErrorMessageConstants.InvalidJwtToken);
        }

        var userInfo = await _dbContext.UserRefreshTokens
            .Where(u => u.UserId == userId.Value && u.RefreshToken == refreshToken)
            .Select(u => new
            {
                TokenCreatedDateTime = u.CreatedDateTime,
                UserStatus = u.User.Status,
                u.User.UserRole
            })
            .FirstOrDefaultAsync();

        if (userInfo is null)
        {
            throw new BadRequestException(ErrorMessageConstants.InvalidRefreshToken);
        }

        var dateTimeUtcNow = DateTimeOffset.UtcNow;

        if ((dateTimeUtcNow - userInfo.TokenCreatedDateTime.UtcDateTime).TotalMinutes > _appSettings.RefreshTokenLifetimeMinutes)
        {
            throw new BadRequestException(ErrorMessageConstants.InvalidRefreshToken);
        }

        if (userInfo.UserStatus is UserStatus.Inactive)
        {
            throw new BadRequestException(ErrorMessageConstants.InvalidRefreshToken);
        }

        var userTokenArgs = new JwtTokenArgsModel
        {
            UserId = userId.Value,
            UserRole = userInfo.UserRole,
            UserStatus = userInfo.UserStatus
        };

        var tokens = new TokensModel
        {
            JwtToken = GenerateJwtToken(userTokenArgs),
            RefreshToken = await CreateRefreshTokenAsync(userId.Value)
        };

        return tokens;
    }

    private SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
        var symmetricSecurityKey = new SymmetricSecurityKey(key);
        return symmetricSecurityKey;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static ClaimsIdentity CreateClaimsIdentity(JwtTokenArgsModel jwtTokenArgsModel)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypeConstants.UserId, jwtTokenArgsModel.UserId.ToString()),
            new(ClaimTypes.Role, jwtTokenArgsModel.UserRole.ToString()),
            new(ClaimTypeConstants.UserStatus, jwtTokenArgsModel.UserStatus.ToString())
        };

        return new ClaimsIdentity(claims);
    }

    private long? ValidateExpiredJwtToken(string expiredJwtToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = GetSymmetricSecurityKey();

            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symmetricSecurityKey,
                ValidateLifetime = false
            };

            tokenHandler.ValidateToken(expiredJwtToken, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = long.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypeConstants.UserId).Value);
            return userId;
        }
        catch
        {
            return null;
        }
    }
}