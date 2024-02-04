using Identity.Application.Interface;
using Identity.Persistance;
using Identity.Persistance.Identity;
using Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;

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
        SeedDataExtention.Initialize(app.ApplicationServices).Wait();
    }

}

public static class SeedDataExtention
{
    private static readonly string[] Roles = new string[] { "Admin", "Manager", "Member" };


    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var _roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await _roleManager.Roles.AnyAsync() is false)
        {
            foreach (var role in Roles)
            {
                if (await _roleManager.RoleExistsAsync(role) is false)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            ApplicationUser user = new()
            {
                FirstName = "Name",
                LastName = "LastName",
                UserName = "admin",
                Email = "Owner@example.com",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                JoinedOn = DateTime.Now,
                Culture = 1,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (await _userManager.Users.AnyAsync() is false)
            {
                await _userManager.CreateAsync(user, "987654321`");
            }

            await _userManager.AddToRolesAsync(user, Roles);
        }
    }
}
