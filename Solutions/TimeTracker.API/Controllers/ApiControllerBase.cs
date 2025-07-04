using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTracker.API.Extensions;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Enums;

namespace TimeTracker.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ApiControllerBase : ControllerBase
{
    private long? _userId;
    private UserRole? _role;

    protected long UserId
    {
        get
        {
            _userId ??= User.GetValueFromToken<int>(ClaimTypeConstants.UserId);
            return _userId.Value;
        }
    }

    protected UserRole Role
    {
        get
        {
            _role ??= User.GetEnumValueFromToken<UserRole>(ClaimTypes.Role);
            return _role.Value;
        }
    }
}