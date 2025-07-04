using TimeTracker.Common.Enums;

namespace TimeTracker.BLL.Models.Password;

public class SetPasswordModel
{
    public string Token { get; set; }
    public string Password { get; set; }
    public PasswordActionType PasswordActionType { get; set; }
}