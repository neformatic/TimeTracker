namespace TimeTracker.Common.Models.Settings;

public class AppSettings
{
    public string JwtSecret { get; set; }
    public int JwtTokenLifetimeMinutes { get; set; }
    public int RefreshTokenLifetimeMinutes { get; set; }
    public int SetPasswordTokenLifeTimeMinutes { get; set; }
    public bool IsApplyMigrationsOnStartup { get; set; }
    public int SqlCommandTimeoutSeconds { get; set; }
    public string[] CorsAllowedOrigins { get; set; }
    public int ResetPasswordTokenLifeTimeMinutes { get; set; }
    public string ApplicationLogoLink { get; set; }
}