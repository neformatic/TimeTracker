using TimeTracker.BLL.Models.Password;

namespace TimeTracker.BLL.Helpers.Interfaces;

public interface IPasswordHelper
{
    PasswordValidationResultModel ValidatePassword(string password);
}