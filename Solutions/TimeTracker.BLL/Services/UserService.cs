using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeTracker.BLL.Models.User;
using TimeTracker.BLL.Models.User.Filter;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Enums;
using TimeTracker.Common.Exceptions;
using TimeTracker.Common.Helpers.Interfaces;
using TimeTracker.Common.Models;
using TimeTracker.DAL;
using TimeTracker.DAL.Entities;
using TimeTracker.DAL.Extensions;

namespace TimeTracker.BLL.Services;

public class UserService : IUserService
{
    private readonly TimeTrackerDbContext _dbContext;
    private readonly IPaginationHelper _paginationHelper;
    private readonly IMapper _mapper;

    public UserService(TimeTrackerDbContext dbContext,
        IPaginationHelper paginationHelper,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _paginationHelper = paginationHelper;
        _mapper = mapper;
    }


    public async Task<UserModel> CreateUserAsync(CreateUserModel createUserModel)
    {
        var dateTimeUtcNow = DateTimeOffset.UtcNow;

        //var createUserEntity = _mapper.Map<User>(createUserModel);

        var createUserEntity = new User
        {
            Name = createUserModel.Name,
            Password = createUserModel.Password,
            Email = createUserModel.Email,
            UserRole = createUserModel.UserRole,
            CreatedDateTime = dateTimeUtcNow,
            UpdatedDateTime = dateTimeUtcNow,
            Status = UserStatus.Active
        };

        await _dbContext.ExecuteInTransactionAsync(async () =>
        {
            _dbContext.Users.Add(createUserEntity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.CheckPostgresUniqueConstraintException())
            {
                throw new BadRequestException(ErrorMessageConstants.ContactEmailAlreadyInUse);
            }
        });

        var createdUserModel = await GetUserAsync(createUserEntity.Id);

        return createdUserModel;
    }

    public async Task<UserModel> UpdateUserAsync(UpdateUserModel updateUserModel)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == updateUserModel.Id);

        if (user is null)
        {
            throw new EntityNotFoundException(updateUserModel.Id);
        }

        user.Name = updateUserModel.Name;
        user.Email = updateUserModel.Email;
        user.UpdatedDateTime = DateTimeOffset.UtcNow;

        await _dbContext.ExecuteInTransactionAsync(async () =>
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.CheckPostgresUniqueConstraintException())
            {
                throw new BadRequestException(ErrorMessageConstants.ContactEmailAlreadyInUse);
            }
        });

        var userModel = await GetUserAsync(user.Id);

        return userModel;
    }

    public async Task<UserModel> GetUserAsync(long id)
    {
        var userModel = await _dbContext.Users
            .Where(u => u.Id == id)
            .Select(u => new UserModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password
            })
            .FirstOrDefaultAsync();

        if (userModel is null)
        {
            throw new EntityNotFoundException(id);
        }

        return userModel;
    }

    public async Task<PaginationResultModel<UserModel>> GetUsersListAsync(UserFilterModel filter)
    {
        var userModels = await _dbContext.Users
            .Select(u => new UserModel
            {
                Id = u.Id,
                Name = u.Name,
                Password = u.Password,
                Email = u.Email
            })
            .ToListAsync();

        var paginationResult = _paginationHelper.GeneratePaginationResultModel(userModels, filter.PageNumber, filter.PageItemsCount
        );

        return paginationResult;
    }
}