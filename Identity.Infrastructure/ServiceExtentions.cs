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
        using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetService<AppIdentityDbContext>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //if (!dbContext.Database.GetPendingMigrations().Any())
            //{
            //    await dbContext.Database.MigrateAsync();
            if (!await dbContext.Roles.AnyAsync())
            {
                //seed roles
                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                //seed users
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

                if (!await dbContext.Users.AnyAsync(u => u.UserName == user.UserName))
                {
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(user, "987654321`");
                    user.PasswordHash = hashed;

                    var userStore = new UserStore<ApplicationUser>(dbContext);
                    var result = await userStore.CreateAsync(user);
                }

                await _userManager.AddToRolesAsync(user, Roles);
                await dbContext.SaveChangesAsync();

            }
        }
    }
}
