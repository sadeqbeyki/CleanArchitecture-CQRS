using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application;

public static class ServiceExtentions
{
    public static void AddIdentityApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf => 
            conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
