namespace TimeTracker.BLL.Models.Login;

public class LoginResultModel
{
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
}