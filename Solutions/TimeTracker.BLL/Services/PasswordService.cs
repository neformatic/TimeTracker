using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TimeTracker.BLL.Helpers.Interfaces;
using TimeTracker.BLL.Models.Password;
using TimeTracker.BLL.Models.User;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Enums;
using TimeTracker.Common.Exceptions;
using TimeTracker.Common.Models.Settings;
using TimeTracker.DAL;
using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Models.User;

namespace TimeTracker.BLL.Services;

public class PasswordService : IPasswordService
{
    private const string ResetPasswordEmailSubject = "Reset Password";
    private const string ResetPasswordPath = "reset-password";

    private const string SendCredentialsEmailSubject = "TimeTracker: Credentials";
    private const string SetPasswordPath = "set-password";

    private readonly DateTimeOffset _lastTokenCreationTime = DateTimeOffset.UtcNow.AddHours(-1);

    private readonly TimeTrackerDbContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly IPasswordHelper _passwordHelper;
    private readonly IHashHelper _hashHelper;
    private readonly IEmailTemplateHelper _emailTemplateHelper;
    private readonly AppSettings _appSettings;

    public PasswordService(TimeTrackerDbContext dbContext,
        IEmailService emailService,
        IOptions<AppSettings> appSettings,
        IPasswordHelper passwordHelper,
        IHashHelper hashHelper,
        IEmailTemplateHelper emailTemplateHelper)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        _passwordHelper = passwordHelper;
        _hashHelper = hashHelper;
        _emailTemplateHelper = emailTemplateHelper;
        _appSettings = appSettings.Value;
    }

    public async Task RequestResetPasswordEmailAsync(RequestResetPasswordModel requestResetPasswordModel)
    {
        var user = await _dbContext.Users
            .Where(u => u.Email == requestResetPasswordModel.Email)
            .Select(u => new
            {
                u.Id
            })
            .FirstOrDefaultAsync();

        if (user is null)
        {
            throw new BadRequestException(ErrorMessageConstants.UserWithThatEmailNotExist);
        }

        var token = GenerateToken();
        var dateTimeUtcNow = DateTimeOffset.UtcNow;

        var resetPasswordToken = await _dbContext.ResetPasswordTokens
            .FirstOrDefaultAsync(rpt => rpt.UserId == user.Id);

        if (resetPasswordToken is null)
        {
            resetPasswordToken = new ResetPasswordToken
            {
                UserId = user.Id
            };

            _dbContext.ResetPasswordTokens.Add(resetPasswordToken);
        }

        resetPasswordToken.Token = token;
        resetPasswordToken.IsActive = true;
        resetPasswordToken.CreatedDateTime = dateTimeUtcNow;

        var link = GenerateLink(token, requestResetPasswordModel.AppUrl, ResetPasswordPath);
        var emailMessage = await GenerateResetPasswordEmailMessageAsync(link);

        await _emailService.SendAsync(requestResetPasswordModel.Email, ResetPasswordEmailSubject, emailMessage);

        await _dbContext.SaveChangesAsync();
    }

    public async Task SetPasswordAsync(SetPasswordModel setPasswordModel)
    {
        var dateTimeUtcNow = DateTimeOffset.UtcNow;

        if (!_passwordHelper.ValidatePassword(setPasswordModel.Password).IsValid)
        {
            throw new BadRequestException(ErrorMessageConstants.PasswordDoesntMatchValidation);
        }

        var token = await GetValidatedToken(setPasswordModel);

        token.IsActive = false;
        token.User.Password = _hashHelper.GenerateHash(setPasswordModel.Password);
        token.User.UpdatedDateTime = dateTimeUtcNow;

        await _dbContext.SaveChangesAsync();
    }

    public async Task SendUserCredentialsAsync(SendUserCredentialsModel sendUserCredentialsModel)
    {
        var requestedCredentialsUserRole = sendUserCredentialsModel.RequestedCredentialsUserRole;

        var userInfoDataModel = await _dbContext.Users
            .Where(u => u.Id == sendUserCredentialsModel.UserId
                        && u.UserRole == requestedCredentialsUserRole)
            .Select(u => new UserInfoDataModel
            {
                UserId = u.Id,
                Name = u.Name,
                Email = u.Email,
                UserStatus = u.Status
            })
            .FirstOrDefaultAsync();

        if (userInfoDataModel is null)
        {
            var message = requestedCredentialsUserRole switch
            {
                UserRole.DefaultUser => ErrorMessageConstants.UserNotFound,
                _ => throw new ArgumentOutOfRangeException()
            };

            throw new BadRequestException(message);
        }


        var resetPasswordToken = await _dbContext.ResetPasswordTokens
            .FirstOrDefaultAsync(rpt => rpt.UserId == userInfoDataModel.UserId);

        if (resetPasswordToken is not null && resetPasswordToken.CreatedDateTime > _lastTokenCreationTime)
        {
            throw new BadRequestException(ErrorMessageConstants.EmailSendLimitExceeded);
        }

        var token = GenerateToken();
        var dateTimeUtcNow = DateTimeOffset.UtcNow;

        if (resetPasswordToken is null)
        {
            resetPasswordToken = new ResetPasswordToken
            {
                UserId = userInfoDataModel.UserId
            };

            _dbContext.ResetPasswordTokens.Add(resetPasswordToken);
        }

        resetPasswordToken.Token = token;
        resetPasswordToken.IsActive = true;
        resetPasswordToken.CreatedDateTime = dateTimeUtcNow;

        var link = GenerateLink(token, sendUserCredentialsModel.AppUrl, SetPasswordPath);
        var emailMessage = await GenerateUserCredentialsEmailMessageAsync(link, requestedCredentialsUserRole, userInfoDataModel);

        await _emailService.SendAsync(userInfoDataModel.Email, SendCredentialsEmailSubject, emailMessage);

        UpdateUser(userInfoDataModel, dateTimeUtcNow);

        await _dbContext.SaveChangesAsync();
    }

    private void UpdateUser(UserInfoDataModel userInfoDataModel, DateTimeOffset updatedDateTime)
    {
        var userEntity = new User
        {
            Id = userInfoDataModel.UserId
        };

        _dbContext.Users.Attach(userEntity);

        userEntity.Status = UserStatus.Active;
        userEntity.UpdatedDateTime = updatedDateTime;
    }

    private string GenerateToken()
    {
        return string.Join(string.Empty, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    }

    private string GenerateLink(string token, string appUrl, string path)
    {
        return string.Join(CharConstants.Slash, appUrl, path, token);
    }

    private async Task<string> GenerateResetPasswordEmailMessageAsync(string link)
    {
        var template = await _emailTemplateHelper.GetResetPasswordEmailTemplateAsync();
        template.Content = template.Content.Replace(TemplatePlaceholderConstants.Logo, _appSettings.ApplicationLogoLink);

        var message = template.Content.Replace(TemplatePlaceholderConstants.ResetPasswordLink, link);

        return message;
    }

    private async Task<string> GenerateUserCredentialsEmailMessageAsync(string link,
        UserRole requestedCredentialsUserRole,
        UserInfoDataModel userInfoDataModel)
    {
        var template = await _emailTemplateHelper.GetUserCredentialsEmailTemplateAsync(requestedCredentialsUserRole);

        var userCredentialsPlaceholders = GetUserCredentialsPlaceholders(link, userInfoDataModel);
        var message = _emailTemplateHelper.ReplacePlaceholders(template.Content, userCredentialsPlaceholders);

        return message;
    }

    private Dictionary<string, string> GetUserCredentialsPlaceholders(string link, UserInfoDataModel userInfoDataModel)
    {
        return new Dictionary<string, string>
        {
            { TemplatePlaceholderConstants.Name, userInfoDataModel.Name },
            { TemplatePlaceholderConstants.Email, userInfoDataModel.Email },
            { TemplatePlaceholderConstants.Link, link},
            { TemplatePlaceholderConstants.Logo, _appSettings.ApplicationLogoLink }
        };
    }

    private async Task<ResetPasswordToken> GetValidatedToken(SetPasswordModel setPasswordModel)
    {
        var resetTokenLifetime = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(_appSettings.ResetPasswordTokenLifeTimeMinutes);
        var setTokenLifetime = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(_appSettings.SetPasswordTokenLifeTimeMinutes);

        var token = await _dbContext.ResetPasswordTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == setPasswordModel.Token
                                      && t.IsActive);

        if (token is null)
        {
            throw new BadRequestException(ErrorMessageConstants.PasswordTokenInvalidOrExpired);
        }

        var isEqualToPrevious = _hashHelper.VerifyHash(token.User.Password, setPasswordModel.Password);

        if (isEqualToPrevious)
        {
            throw new BadRequestException(ErrorMessageConstants.NewPasswordIsEqualToPrevious);
        }

        switch (setPasswordModel.PasswordActionType)
        {
            case PasswordActionType.Reset:
                if (token.CreatedDateTime < resetTokenLifetime)
                {
                    throw new BadRequestException(ErrorMessageConstants.ResetPasswordTokenInvalidOrExpired);
                }
                break;

            case PasswordActionType.Set:
                if (token.CreatedDateTime < setTokenLifetime)
                {
                    throw new BadRequestException(string.Format(ErrorMessageConstants.SetPasswordTokenInvalidOrExpiredTemplate));
                }
                break;

            default:
                throw new InvalidOperationException($"Unsupported PasswordActionType: {setPasswordModel.PasswordActionType}");
        }

        return token;
    }
}