using Microsoft.EntityFrameworkCore.Design;
using TimeTracker.Common.Constants;

namespace TimeTracker.DAL.Factories;

public class TimeTrackerDbContextDesignTimeFactory : DbContextDesignTimeFactoryBase, IDesignTimeDbContextFactory<TimeTrackerDbContext>
{
    public TimeTrackerDbContext CreateDbContext(string[] args)
    {
        var options = CreateDbContextOptions<TimeTrackerDbContext>(AppSettingsNameConstants.TimeTrackerConnectionStringName);
        var dbContext = new TimeTrackerDbContext(options);
        return dbContext;
    }
}