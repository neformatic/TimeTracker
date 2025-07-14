using TimeTracker.BLL.Models.User;
using TimeTracker.BLL.Models.User.Filter;
using TimeTracker.Common.Models;

namespace TimeTracker.BLL.Services.Interfaces;

public interface IUserService
{
    Task<UserModel> CreateUserAsync(CreateUserModel createUserModel);
    Task<UserModel> UpdateUserAsync(UpdateUserModel updateUserModel);
    Task<UserModel> GetUserAsync(long id);
    Task<PaginationResultModel<UserModel>> GetUsersListAsync(UserFilterModel filter);
}