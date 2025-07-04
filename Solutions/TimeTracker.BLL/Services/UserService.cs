using AutoMapper;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.DAL;

namespace TimeTracker.BLL.Services;

public class UserService : IUserService
{
    private readonly TimeTrackerDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserService(TimeTrackerDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
}