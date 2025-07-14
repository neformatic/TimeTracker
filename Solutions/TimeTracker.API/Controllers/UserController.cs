using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.ViewModels.Common;
using TimeTracker.API.ViewModels.User;
using TimeTracker.API.ViewModels.User.Filter;
using TimeTracker.BLL.Models.User;
using TimeTracker.BLL.Models.User.Filter;
using TimeTracker.BLL.Services.Interfaces;

namespace TimeTracker.API.Controllers;

public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetUser(long id)
    {
        var userModel = await _userService.GetUserAsync(id);
        var userViewModel = _mapper.Map<UserViewModel>(userModel);

        return Ok(userViewModel);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserViewModel createUserViewModel)
    {
        var createUserModel = _mapper.Map<CreateUserModel>(createUserViewModel);
        var createdUserModel = await _userService.CreateUserAsync(createUserModel);
        var userViewModel = _mapper.Map<UserViewModel>(createdUserModel);

        return Ok(userViewModel);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserViewModel updateUserViewModel)
    {
        var updateUserModel = _mapper.Map<UpdateUserModel>(updateUserViewModel);
        var userModel = await _userService.UpdateUserAsync(updateUserModel);
        var userViewModel = _mapper.Map<UserViewModel>(userModel);

        return Ok(userViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersList([FromQuery] UserFilterViewModel filter)
    {
        var userFilterModel = _mapper.Map<UserFilterModel>(filter);
        var usersList = await _userService.GetUsersListAsync(userFilterModel);
        var usersListViewModel = _mapper.Map<PaginationResultViewModel<UserViewModel>>(usersList);

        return Ok(usersListViewModel);
    }
}