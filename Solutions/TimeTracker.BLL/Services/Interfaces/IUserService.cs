using TimeTracker.BLL.Models.User;
using TimeTracker.BLL.Models.User.Filter;
using TimeTracker.Common.Models;

namespace TimeTracker.BLL.Services.Interfaces;

public interface IUserService
{
    Task<UserModel> CreateAsync(CreateUserModel createUserModel);
    Task<UserModel> UpdateAsync(UpdateUserModel updateUserModel);
    Task<UserModel> GetAsync(long id);
    Task<PaginationResultModel<UserModel>> GetUsersListAsync(UserFilterModel filter);
}