using AutoMapper;
using TimeTracker.BLL.Models.User;
using TimeTracker.DAL.Entities;

namespace TimeTracker.BLL.Mapping;

public class UserProfileMapping : Profile
{
    public UserProfileMapping()
    {
        CreateMap<CreateUserModel, User>().ReverseMap();
        CreateMap<UpdateUserModel, User>();
    }
}