using TimeTracker.Common.Enums;

namespace TimeTracker.DAL.Models.User;

public class UserInfoDataModel
{
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string AssociationUrlPathEnding { get; set; }
    public UserStatus UserStatus { get; set; }
}