using TimeTracker.BLL.Models.Token;

namespace TimeTracker.BLL.Services.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(JwtTokenArgsModel jwtTokenArgsModel);
    Task<string> CreateRefreshTokenAsync(long userId);
    Task<TokensModel> RefreshTokensAsync(string expiredJwtToken, string refreshToken);
}