using TimeTracker.Common.Contexts.Interfaces;
using TimeTracker.Common.Enums;

namespace TimeTracker.Common.Contexts;

public class AuthorizationContext : IAuthorizationContext
{
    public long UserId { get; init; }
    public UserRole UserRole { get; init; }
}