using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TimeTracker.API.Extensions;
using TimeTracker.API.Helpers;
using TimeTracker.API.Helpers.Interfaces;
using TimeTracker.BLL.Helpers;
using TimeTracker.BLL.Helpers.Interfaces;
using TimeTracker.BLL.Services;
using TimeTracker.BLL.Services.Interfaces;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Contexts;
using TimeTracker.Common.Contexts.Interfaces;
using TimeTracker.Common.Enums;
using TimeTracker.Common.Helpers;
using TimeTracker.Common.Helpers.Interfaces;
using TimeTracker.Common.Models.Settings;
using TimeTracker.DAL;

namespace TimeTracker.API.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddDependencyInjection(this IServiceCollection services,
        ConfigurationManager configurationManager,
        AppSettings appSettings)
    {
        InitDatabaseContexts(services, configurationManager, appSettings);
        InitApiServices(services);
        InitBllServices(services);
    }

    private static void InitDatabaseContexts(IServiceCollection services, ConfigurationManager configurationManager, AppSettings appSettings)
    {
        var connectionString = configurationManager.GetConnectionString(AppSettingsNameConstants.TimeTrackerConnectionStringName);

        services.AddDbContext<TimeTrackerDbContext>(config =>
        {
            config.UseNpgsql(connectionString, options => options
                .CommandTimeout(appSettings.SqlCommandTimeoutSeconds)
                .EnableRetryOnFailure());
        });

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    private static void InitApiServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthCookieHelper, AuthCookieHelper>();

    }

    private static void InitBllServices(IServiceCollection services)
    {
        AddAuthorizationContext(services);

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITrackService, TrackService>();

        services.AddScoped<IHashHelper, HashHelper>();
        services.AddScoped<IPasswordHelper, PasswordHelper>();
        services.AddScoped<IPaginationHelper, PaginationHelper>();

        services.AddScoped<IEmailTemplateHelper, EmailTemplateHelper>();
        services.AddScoped<ITemplateHelper, TemplateHelper>();
    }

    private static void AddAuthorizationContext(IServiceCollection services)
    {
        services.AddScoped<IAuthorizationContext, AuthorizationContext>(sp =>
        {
            var httpContextAccessor = sp.GetService<IHttpContextAccessor>();

            var user = httpContextAccessor.HttpContext?.User;

            AuthorizationContext authorizationContext = null;

            if (user?.Identity?.IsAuthenticated == true)
            {
                authorizationContext = new AuthorizationContext
                {
                    UserId = user.GetValueFromToken<int>(ClaimTypeConstants.UserId),
                    UserRole = user.GetEnumValueFromToken<UserRole>(ClaimTypes.Role)
                };
            }

            return authorizationContext;
        });
    }
}