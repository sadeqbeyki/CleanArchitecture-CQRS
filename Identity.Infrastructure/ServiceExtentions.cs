using Identity.Application.Interface;
using Identity.Persistance;
using Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class ServiceExtentions
{
    public static void AddIdentityInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppIdentityDbContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

        services.AddScoped<IIdentityService, IdentityService>();

    }
    public static void CreateIdentityDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        dataContext.Database.EnsureCreated();

        //dataContext.Database.Migrate();
    }
    public static void SeedData(this IApplicationBuilder app)
    {
        SeedDataExtention.SeedRoles(app.ApplicationServices).Wait();
    }

}

public static class SeedDataExtention
{
    private static readonly string[] Roles = new string[] { "Admin", "Manager", "Member" };

    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetService<AppIdentityDbContext>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //if (!dbContext.Database.GetPendingMigrations().Any())
            //{
            //    await dbContext.Database.MigrateAsync();
            if (!dbContext.Roles.Any())
            {
                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }
    }
}
