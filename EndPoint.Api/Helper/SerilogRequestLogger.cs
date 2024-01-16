using Serilog.Context;
using Serilog;
using System.Text;

namespace EndPoint.Api.Helper;

public class SerilogRequestLogger
{
    readonly RequestDelegate _next;

    public SerilogRequestLogger(RequestDelegate next)
    {
        if (next == null) throw new ArgumentNullException(nameof(next));
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

        // Push the user name into the log context so that it is included in all log entries
        LogContext.PushProperty("UserName", httpContext.User.Identity.Name);

        // Getting the request body is a little tricky because it's a stream
        // So, we need to read the stream and then rewind it back to the beginning
        string requestBody = "";
        HttpRequestRewindExtensions.EnableBuffering(httpContext.Request);
        Stream body = httpContext.Request.Body;
        byte[] buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];
        await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
        requestBody = Encoding.UTF8.GetString(buffer);
        body.Seek(0, SeekOrigin.Begin);
        httpContext.Request.Body = body;

        Log.ForContext("RequestHeaders", httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
           .ForContext("RequestBody", requestBody)
           .Debug("Request information {RequestMethod} {RequestPath} information", httpContext.Request.Method, httpContext.Request.Path);

        Log.Information(string.Format("Request Body: {0} ", requestBody));

        var exampleUser = new WebApplication2User { Id = "1001", UserName = "Adam", SecurityStamp = DateTime.Now.ToString() };
        Log.Information("Created {@User} on {Created}", exampleUser, DateTime.Now);
        // The reponse body is also a stream so we need to:
        // - hold a reference to the original response body stream
        // - re-point the response body to a new memory stream
        // - read the response body after the request is handled into our memory stream
        // - copy the response in the memory stream out to the original response stream
        using (var responseBodyMemoryStream = new MemoryStream())
        {
            var originalResponseBodyReference = httpContext.Response.Body;
            httpContext.Response.Body = responseBodyMemoryStream;

            await _next(httpContext);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

            Log.ForContext("RequestBody", requestBody)
               .ForContext("ResponseBody", responseBody)
               .Debug("Response information {RequestMethod} {RequestPath} {statusCode}", httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode);

            await responseBodyMemoryStream.CopyToAsync(originalResponseBodyReference);
        }
    }
}

