using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TimeTracker.DAL.Factories;

public abstract class DbContextDesignTimeFactoryBase
{
    protected DbContextOptions<T> CreateDbContextOptions<T>(string connectionStringName)
        where T : DbContext
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddEnvironmentVariables();

        var config = configurationBuilder.Build();
        var connectionString = config.GetConnectionString(connectionStringName);
        var optionsBuilder = new DbContextOptionsBuilder<T>();
        optionsBuilder.UseNpgsql(connectionString, opt => opt.CommandTimeout(300));

        return optionsBuilder.Options;
    }
}