using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL;

public class TimeTrackerDbContext : DbContext
{
    public DbSet<TimeEntry> TimeEntries { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<UserRoleEntity> UserStatuses { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

    public TimeTrackerDbContext(DbContextOptions options) : base(options)
    {
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        await Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            if (Database.CurrentTransaction is not null)
            {
                await action();
            }
            else
            {
                await using var transaction = await Database.BeginTransactionAsync();

                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimeTrackerDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
}