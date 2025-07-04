using TimeTracker.Common.Enums;

namespace TimeTracker.BLL.Models.User;

public class SendUserCredentialsModel
{
    public int UserId { get; set; }
    public string AppUrl { get; set; }
    public UserRole RequestedCredentialsUserRole { get; set; }
}