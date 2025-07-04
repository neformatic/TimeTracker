using System.Reflection;
using TimeTracker.API.Infrastructure;
using TimeTracker.API.Middleware;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = !builder.Environment.IsProduction();
var appSettings = builder.Services.AddSettingsConfiguration(builder);

builder.Services.AddCors(appSettings);
builder.Services.AddDependencyInjection(builder.Configuration, appSettings);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddJwtAuthentication(appSettings.JwtSecret);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(isDevelopment);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(CorsExtension.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();