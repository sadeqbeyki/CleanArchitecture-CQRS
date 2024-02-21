using Serilog;
using Serilog.Events;

namespace WebAPI.Helper;

public static class ApplicationBuilderHelper
{
    public static void UseSwaggerCustom(this IApplicationBuilder app, IConfiguration configuration)
    {
        var apiVersion = configuration["SwaggerDetails:ApiVersion"];

        // Enable middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger(c =>
        {
            c.RouteTemplate = configuration["SwaggerDetails:Template"];
        });

        // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
        app.UseSwaggerUI(options =>
        {
            options.EnableFilter();
            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            options.DisplayRequestDuration();
            options.RoutePrefix = configuration["SwaggerDetails:RoutePrefix"];

            options.SwaggerEndpoint(
                configuration["SwaggerDetails:Endpoints:API:Url"],
                configuration["SwaggerDetails:Endpoints:API:Name"]);
        });
    }

    public static void CustomSerilogLogging(this IApplicationBuilder app)
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
