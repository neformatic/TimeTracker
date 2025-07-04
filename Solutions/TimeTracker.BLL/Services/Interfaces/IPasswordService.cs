using TimeTracker.BLL.Models.Password;
using TimeTracker.BLL.Models.User;

namespace TimeTracker.BLL.Services.Interfaces;

public interface IPasswordService
{
    Task RequestResetPasswordEmailAsync(RequestResetPasswordModel requestResetPasswordModel);
    Task SetPasswordAsync(SetPasswordModel setPasswordModel);
    Task SendUserCredentialsAsync(SendUserCredentialsModel sendUserCredentialsModel);
}