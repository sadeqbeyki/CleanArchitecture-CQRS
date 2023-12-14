using Identity.Application.Interface;
using Identity.Persistance;
using Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class ServiceExtentions
{
    public static void AddIdentityInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

        services.AddScoped<IdentityDbContext>();
        services.AddScoped<IIdentityService, IdentityService>();

    }
    public static void CreateIdentityDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        dataContext.Database.EnsureCreated();
        //dataContext.Database.Migrate();
    }
}
