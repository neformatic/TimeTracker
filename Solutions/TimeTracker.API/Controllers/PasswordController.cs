using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.ViewModels.Password;
using TimeTracker.BLL.Models.Password;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.Common.Enums;

namespace TimeTracker.API.Controllers;

public class PasswordController : ApiControllerBase
{
    private const string OriginHeaderIsMissing = "Origin header is missing";
    
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;

    public PasswordController(IPasswordService passwordService,
        IMapper mapper)
    {
        _passwordService = passwordService;
        _mapper = mapper;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RequestResetPassword(RequestResetPasswordViewModel requestResetPasswordViewModel)
    {
        var appUrl = GetAppUrl();

        var requestResetPasswordModel = _mapper.Map<RequestResetPasswordModel>(requestResetPasswordViewModel);
        requestResetPasswordModel.AppUrl = appUrl;

        await _passwordService.RequestResetPasswordEmailAsync(requestResetPasswordModel);
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ResetPassword(SetPasswordViewModel setPasswordViewModel)
    {
        var setPasswordModel = _mapper.Map<SetPasswordModel>(setPasswordViewModel);
        setPasswordModel.PasswordActionType = PasswordActionType.Reset;

        await _passwordService.SetPasswordAsync(setPasswordModel);

        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel setPasswordViewModel)
    {
        var setPasswordModel = _mapper.Map<SetPasswordModel>(setPasswordViewModel);
        setPasswordModel.PasswordActionType = PasswordActionType.Set;

        await _passwordService.SetPasswordAsync(setPasswordModel);

        return Ok();
    }
    
    private string GetAppUrl()
    {
        var appUrl = HttpContext.Request.Headers.Origin.FirstOrDefault();

        if (appUrl is null)
        {
            throw new Exception(OriginHeaderIsMissing);
        }

        return appUrl;
    }
}