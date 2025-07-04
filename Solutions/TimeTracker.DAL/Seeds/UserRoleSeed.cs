using TimeTracker.Common.Enums;
using TimeTracker.Common.Extensions;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Seeds;

public static class UserRoleSeed
{
    public static IEnumerable<UserRoleEntity> UserRoles
    {
        get
        {
            foreach (var userRole in (UserRole[])Enum.GetValues(typeof(UserRole)))
            {
                yield return new UserRoleEntity
                {
                    Id = userRole,
                    Description = userRole.GetDescription()
                };
            }
        }
    }
}