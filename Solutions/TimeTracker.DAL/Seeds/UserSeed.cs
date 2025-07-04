using TimeTracker.Common.Enums;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Seeds;

public static class UserSeed
{
    public static IEnumerable<User> Users
    {
        get
        {
            yield return new User
            {
                Id = -1,
                Password = string.Empty,
                Name = "System",
                Email = string.Empty,
                UserRole = UserRole.SuperAdmin,
                Status = UserStatus.Active
            };

            yield return new User
            {
                Id = 1,
                Password = "AQAAAAIAAAPoAAAAEIRkNwshX5CX4zRwq4S/s2TTaF7POQ6KciTxA3F7EYykjsSV7QU2g021W6hFKNoM8w==",
                Name = "TEST_NAME_1",
                Email = "test1@email.com",
                UserRole = UserRole.DefaultUser,
                Status = UserStatus.Active
            };

            yield return new User
            {
                Id = 2,
                Password = "AQAAAAIAAAPoAAAAEIRkNwshX5CX4zRwq4S/s2TTaF7POQ6KciTxA3F7EYykjsSV7QU2g021W6hFKNoM8w==",
                Name = "TEST_NAME_2",
                Email = "test2@email.com",
                UserRole = UserRole.DefaultUser,
                Status = UserStatus.Active
            };

            yield return new User
            {
                Id = 3,
                Password = "AQAAAAIAAAPoAAAAEIRkNwshX5CX4zRwq4S/s2TTaF7POQ6KciTxA3F7EYykjsSV7QU2g021W6hFKNoM8w==",
                Name = "TEST_NAME_3",
                Email = "test3@email.com",
                UserRole = UserRole.DefaultUser,
                Status = UserStatus.Active
            };
        }
    }
}