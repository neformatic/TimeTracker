using TimeTracker.Common.Models.Settings;

namespace TimeTracker.API.Infrastructure;

public static class CorsExtension
{
    public const string CorsPolicyName = "CorsPolicyName";

    public static void AddCors(this IServiceCollection services,
        AppSettings appSettings)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, builder =>
            {
                builder.SetIsOriginAllowed(origin => appSettings.CorsAllowedOrigins.Any(x => x == new Uri(origin).Host))
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
            });
        });
    }
}