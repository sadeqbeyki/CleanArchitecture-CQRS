﻿using Identity.Application.Interface;
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
}
