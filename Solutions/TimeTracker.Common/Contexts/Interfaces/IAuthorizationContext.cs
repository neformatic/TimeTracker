using TimeTracker.Common.Enums;

namespace TimeTracker.Common.Contexts.Interfaces;

public interface IAuthorizationContext
{
    long UserId { get; init; }
    UserRole UserRole { get; init; }
}