using Microsoft.OpenApi.Models;

namespace TimeTracker.API.Infrastructure;

public static class SwaggerExtension
{
    private const string Title = "Time tracker Api";

    public static void AddSwagger(this IServiceCollection services, bool isDevelopment)
    {
        if (isDevelopment)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Title
                });
            });
        }
    }
}