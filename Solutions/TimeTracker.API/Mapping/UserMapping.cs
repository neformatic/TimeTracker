using AutoMapper;
using TimeTracker.API.ViewModels.User;
using TimeTracker.BLL.Models.User;

namespace TimeTracker.API.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<CreateUserViewModel, CreateUserModel>();
        CreateMap<UpdateUserViewModel, UpdateUserModel>();
        CreateMap<UserViewModel, UserModel>().ReverseMap();
    }
}