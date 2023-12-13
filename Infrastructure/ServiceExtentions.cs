using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance;

namespace Infrastructure;

public static class ServiceExtentions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ProductDbContext>(options =>
        {
            options.UseSqlServer("Data Source=localhost;Initial Catalog=NadinTestDB;Integrated Security=True");
        });
    }
    public static void CreateDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>();
        dataContext.Database.EnsureCreated();
        //dataContext.Database.Migrate();
    }
}
