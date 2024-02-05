using EndPoint.Api.Middlewares.Model;
using System.Net;

namespace EndPoint.Api.Middlewares;

public class LogExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogExceptionMiddleware> _logger;
    public LogExceptionMiddleware(RequestDelegate next, ILogger<LogExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
            //_logger.LogInformation("Everything is Ok.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var message = exception switch
        {
            AccessViolationException => "Access violation error from the custom middleware",
            _ => "Internal Server Error from the custom middleware."
        };

        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = message /*"Internal Server Error from the custom middleware."*/
        }.ToString());
    }
}

public static class LogExceptionMiddlewareExtensions
{
    public static void ConfigureLogExceptionMiddleware(this IApplicationBuilder appBuilder)
    {
        appBuilder.UseMiddleware<LogExceptionMiddleware>();
    }

    //public static void ConfigureLogExceptionMiddleware(this WebApplication app)
    //{
    //    app.UseMiddleware<LogExceptionMiddleware>();
    //}
}