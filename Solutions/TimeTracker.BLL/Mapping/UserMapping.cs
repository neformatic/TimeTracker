using AutoMapper;
using TimeTracker.BLL.Models.User;
using TimeTracker.DAL.Entities;

namespace TimeTracker.BLL.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<CreateUserModel, User>();
        CreateMap<UpdateUserModel, User>();
        CreateMap<UserModel, User>().ReverseMap();
    }
}