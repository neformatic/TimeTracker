using TimeTracker.Common.Models.Settings;

namespace TimeTracker.API.Infrastructure;

public static class SettingsStartupExtension
{
    public static AppSettings AddSettingsConfiguration(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        var appSettingsSection = builder.Configuration.GetSection(nameof(AppSettings));
        services.Configure<AppSettings>(appSettingsSection);

        var emailSettingsSection = builder.Configuration.GetSection(nameof(EmailSettings));
        services.Configure<EmailSettings>(emailSettingsSection);

        return appSettingsSection.Get<AppSettings>();
    }
}