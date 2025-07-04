using Microsoft.EntityFrameworkCore;
using TimeTracker.BLL.Helpers.Interfaces;
using TimeTracker.BLL.Models.Login;
using TimeTracker.BLL.Models.Token;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Enums;
using TimeTracker.Common.Exceptions;
using TimeTracker.DAL;

namespace TimeTracker.BLL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly TimeTrackerDbContext _dbContext;
    private readonly IHashHelper _hashHelper;
    private readonly ITokenService _tokenService;

    public AuthenticationService(TimeTrackerDbContext dbContext,
        IHashHelper hashHelper,
        ITokenService tokenService)
    {
        _dbContext = dbContext;
        _hashHelper = hashHelper;
        _tokenService = tokenService;
    }

    public async Task<LoginResultModel> AuthenticateAsync(LoginModel loginModel)
    {
        var user = await _dbContext.Users
            .Where(u => u.Email == loginModel.Email)
            .FirstOrDefaultAsync();

        if (user == null || !_hashHelper.VerifyHash(user.Password, loginModel.Password))
        {
            throw new BadRequestException(ErrorMessageConstants.InvalidPasswordOrEmail);
        }

        switch (user.Status)
        {
            case UserStatus.Inactive:
                throw new BadRequestException(
                    string.Format(ErrorMessageConstants.InactiveUserLogin));
        }

        var tokenInfo = new JwtTokenArgsModel
        {
            UserId = user.Id,
            UserRole = user.UserRole,
            UserStatus = user.Status
        };

        var loginResult = new LoginResultModel
        {
            JwtToken = _tokenService.GenerateJwtToken(tokenInfo),
            RefreshToken = await _tokenService.CreateRefreshTokenAsync(user.Id)
        };

        return loginResult;
    }
}