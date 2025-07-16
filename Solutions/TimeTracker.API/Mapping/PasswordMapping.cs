using AutoMapper;
using TimeTracker.API.ViewModels.Password;
using TimeTracker.BLL.Models.Password;

namespace TimeTracker.API.Mapping;

public class PasswordMapping : Profile
{
    public PasswordMapping()
    {
        CreateMap<RequestResetPasswordViewModel, RequestResetPasswordModel>();
        CreateMap<SetPasswordViewModel, SetPasswordModel>();
    }
}