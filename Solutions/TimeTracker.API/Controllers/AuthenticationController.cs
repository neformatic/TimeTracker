using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Helpers.Interfaces;
using TimeTracker.API.ViewModels.Login;
using TimeTracker.BLL.Models.Login;
using TimeTracker.BLL.Services.Interfaces;

namespace TimeTracker.API.Controllers;

public class AuthenticationController : ApiControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IAuthCookieHelper _authCookieHelper;
    private readonly IMapper _mapper;

    public AuthenticationController(IAuthenticationService authenticationService,
        IAuthCookieHelper authCookieHelper,
        IMapper mapper)
    {
        _authenticationService = authenticationService;
        _authCookieHelper = authCookieHelper;
        _mapper = mapper;
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel login)
    {
        var loginModel = _mapper.Map<LoginModel>(login);
        var loginResult = await _authenticationService.AuthenticateAsync(loginModel);
        _authCookieHelper.SetAuthCookies(Response, loginResult.JwtToken, loginResult.RefreshToken);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        _authCookieHelper.ClearAuthCookies(Response);
        return Ok();
    }
}