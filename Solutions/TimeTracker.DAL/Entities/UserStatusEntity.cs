using TimeTracker.Common.Enums;

namespace TimeTracker.DAL.Entities;

public class UserStatusEntity : BaseEnumEntity<UserStatus>
{
    public UserStatusEntity()
    {
        Users = [];
    }

    public ICollection<User> Users { get; set; }
}