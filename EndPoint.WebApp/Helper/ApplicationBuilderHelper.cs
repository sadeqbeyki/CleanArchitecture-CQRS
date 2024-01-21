using Serilog;
using Serilog.Events;

namespace EndPoint.WebApp.Helper
{
    public static class ApplicationBuilderHelper
    {
        public static void UseCustomSerilogLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = (httpContext, elapsed, ex) =>
                {
                    if (ex != null || httpContext.Response.StatusCode >= 500)
                    {
                        return LogEventLevel.Error;
                    }

                    return LogEventLevel.Information;
                };
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    var request = httpContext.Request;
                    //diagnosticContext.Set("RequestUrl", $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");

                    diagnosticContext.Set("Scheme", $"{request.Scheme}");
                    diagnosticContext.Set("Host", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestPath", httpContext.Request.Path);
                    diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
                    diagnosticContext.Set("StatusCode", httpContext.Response.StatusCode);
                    diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress);

                };
            });
        }
    }
}
