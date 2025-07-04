using TimeTracker.API.Models;

namespace TimeTracker.API.Helpers.Interfaces;

public interface IAuthCookieHelper
{
    void SetAuthCookies(HttpResponse response, string jwtToken, string refreshToken);
    AuthCookiesModel GetAuthCookies(HttpRequest request);
    void ClearAuthCookies(HttpResponse response);
}