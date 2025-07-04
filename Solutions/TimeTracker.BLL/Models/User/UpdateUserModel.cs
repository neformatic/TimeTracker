using TimeTracker.Common.Enums;

namespace TimeTracker.BLL.Models.User;

public class UpdateUserModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole UserRole { get; set; }
}