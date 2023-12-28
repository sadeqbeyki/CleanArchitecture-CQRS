using Application.Interface.Query;
using Application.Mapper;
using Domain.Entities.Products;
using Domain.Interface;
using Domain.Interface.Queries;
using Identity.Application.Mapper;
using Infrastructure.ACL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance;
using Persistance.Repositories;
using Persistance.Repositories.Query;
using Services.Queries;

namespace Infrastructure;

public static class ServiceExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProductDbContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductDbContext, ProductDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        services.AddScoped<IProductQueryService, ProductQueryService>();

        services.AddScoped<IUserServiceACL, UserServiceACL>();

    }
    public static void CreateDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<ProductDbContext>();
        dataContext.Database.EnsureCreated();
        //dataContext.Database.Migrate();
    }
}
