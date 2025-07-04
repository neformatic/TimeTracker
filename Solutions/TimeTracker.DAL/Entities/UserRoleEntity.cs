using TimeTracker.Common.Enums;

namespace TimeTracker.DAL.Entities;

public class UserRoleEntity
{
    public UserRoleEntity()
    {
        Users = [];
    }

    public UserRole Id { get; set; }
    public string Description { get; set; }

    public List<User> Users { get; set; }
}