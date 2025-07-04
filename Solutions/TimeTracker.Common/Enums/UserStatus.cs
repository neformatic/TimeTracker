using System.ComponentModel;

namespace TimeTracker.Common.Enums;

public enum UserStatus
{
    [Description("Inactive")]
    Inactive = 1,

    [Description("Active")]
    Active = 2
}