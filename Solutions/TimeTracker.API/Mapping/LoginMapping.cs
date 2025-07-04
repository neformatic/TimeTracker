using AutoMapper;
using TimeTracker.API.ViewModels.Login;
using TimeTracker.BLL.Models.Login;

namespace TimeTracker.API.Mapping;

public class LoginMapping : Profile
{
    public LoginMapping()
    {
        CreateMap<LoginViewModel, LoginModel>();
    }
}