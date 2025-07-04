using TimeTracker.Common.Enums;

namespace TimeTracker.BLL.Models.Token;

public class JwtTokenArgsModel
{
    public long UserId { get; set; }
    public UserRole UserRole { get; set; }
    public UserStatus UserStatus { get; set; }
}