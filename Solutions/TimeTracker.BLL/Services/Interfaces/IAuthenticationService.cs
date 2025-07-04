using TimeTracker.BLL.Models.Login;

namespace TimeTracker.BLL.Services.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResultModel> AuthenticateAsync(LoginModel loginModel);
}