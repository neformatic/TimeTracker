using TimeTracker.Common.Enums;

namespace TimeTracker.BLL.Models.User;

public class CreateUserModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole UserRole { get; set; } = UserRole.DefaultUser;
}