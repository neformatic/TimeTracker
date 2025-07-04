using System.Security.Authentication;
using System.Security.Claims;

namespace TimeTracker.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static T GetValueFromToken<T>(this ClaimsPrincipal claimsPrincipal, string claimType) where T :
        IParsable<T>
    {
        const string invalidClaimMsgTemplate = "Invalid {0}";

        var claimValue = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;

        return !string.IsNullOrEmpty(claimValue) && T.TryParse(claimValue, null, out T parsedClaimValue)
            ? parsedClaimValue
            : throw new AuthenticationException(string.Format(invalidClaimMsgTemplate, claimType));
    }

    public static T GetEnumValueFromToken<T>(this ClaimsPrincipal claimsPrincipal, string claimType) where T : struct, IConvertible
    {
        return Enum.Parse<T>(claimsPrincipal.GetValueFromToken<string>(claimType));
    }
}