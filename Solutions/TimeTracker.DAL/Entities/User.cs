using TimeTracker.Common.Enums;

namespace TimeTracker.DAL.Entities;

public class User
{
    public User()
    {
        UserRefreshTokens = new List<UserRefreshToken>();
        TimeEntries = new List<TimeEntry>();
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public UserRole UserRole { get; set; }
    public UserStatus Status { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public DateTimeOffset UpdatedDateTime { get; set; }

    public UserRoleEntity UserRoleEntity { get; set; }
    public UserStatusEntity UserStatusEntity { get; set; }
    public ResetPasswordToken ResetPasswordToken { get; set; }
    public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
    public ICollection<TimeEntry> TimeEntries { get; set; }
}