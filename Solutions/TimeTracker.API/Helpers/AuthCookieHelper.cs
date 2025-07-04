using TimeTracker.API.Constants;
using TimeTracker.API.Helpers.Interfaces;
using TimeTracker.API.Models;

namespace TimeTracker.API.Helpers;

public class AuthCookieHelper : IAuthCookieHelper
{
    public void SetAuthCookies(HttpResponse response, string jwtToken, string refreshToken)
    {
        const int daysCount = 1;

        var cookieOptions = CreateCookieOptions();
        cookieOptions.Expires = DateTime.UtcNow.AddDays(daysCount);

        response.Cookies.Append(CookieNameConstants.JwtToken, jwtToken, cookieOptions);
        response.Cookies.Append(CookieNameConstants.RefreshToken, refreshToken, cookieOptions);
    }

    public AuthCookiesModel GetAuthCookies(HttpRequest request)
    {
        return new AuthCookiesModel
        {
            JwtToken = request.Cookies[CookieNameConstants.JwtToken],
            RefreshToken = request.Cookies[CookieNameConstants.RefreshToken]
        };
    }

    public void ClearAuthCookies(HttpResponse response)
    {
        var cookieOptions = CreateCookieOptions();
        response.Cookies.Delete(CookieNameConstants.JwtToken, cookieOptions);
        response.Cookies.Delete(CookieNameConstants.RefreshToken, cookieOptions);
    }

    private static CookieOptions CreateCookieOptions()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true
        };

        return cookieOptions;
    }
}