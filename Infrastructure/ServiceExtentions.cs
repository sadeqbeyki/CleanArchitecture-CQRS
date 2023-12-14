using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance;

namespace Infrastructure;

public static class ServiceExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        //services.AddDbContext<ProductDbContext>(options =>
        //{
        //    options.UseSqlServer("Data Source=localhost;Initial Catalog=NadinSoftDB;Integrated Security=True");
        //});

        services.AddDbContext<ProductDbContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductDbContext, ProductDbContext>();
    }
    public static void CreateDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>();
        dataContext.Database.EnsureCreated();
        //dataContext.Database.Migrate();
    }
}
