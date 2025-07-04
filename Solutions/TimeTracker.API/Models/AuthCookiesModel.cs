namespace TimeTracker.API.Models;

public class AuthCookiesModel
{
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}