using System.ComponentModel;

namespace TimeTracker.Common.Enums;

public enum UserRole
{
    [Description("Default User")]
    DefaultUser = 1,

    [Description("Super Admin")]
    SuperAdmin = 2
}