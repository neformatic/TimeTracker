using System.Net;
using System.Net.Mime;
using System.Text.Json;
using TimeTracker.API.Extensions;
using TimeTracker.API.ViewModels.Common;
using TimeTracker.Common.Constants;
using TimeTracker.Common.Enums;
using TimeTracker.Common.Exceptions;

namespace TimeTracker.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadRequestException ex)
        {
            await CreateResponseAsync(context, HttpStatusCode.BadRequest, ex.Message, ex.MessageLevel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await CreateResponseAsync(context, HttpStatusCode.InternalServerError, ErrorMessageConstants.SomethingWentWrong);
        }
    }

    private static Task CreateResponseAsync(HttpContext context,
        HttpStatusCode statusCode,
        string errorMessage,
        BadRequestMessageLevel messageLevel = BadRequestMessageLevel.Error)
    {
        var response = context.Response;
        response.ContentType = MediaTypeNames.Application.Json;
        response.StatusCode = (int)statusCode;

        var httpResponseError = new HttpResponseErrorViewModel
        {
            ErrorMessage = errorMessage,
            ErrorMessageLevel = messageLevel
        };

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = JsonSerializer.Serialize(httpResponseError, serializeOptions);
        return response.WriteAsync(result);
    }

    private int? GetUserId(HttpContext context)
    {
        try
        {
            return context.User.GetValueFromToken<int>(ClaimTypeConstants.UserId);
        }
        catch
        {
            return null;
        }
    }
}