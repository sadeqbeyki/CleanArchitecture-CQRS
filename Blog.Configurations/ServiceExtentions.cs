using Blog.Application.Interfaces;
using Blog.Persistance.Common;
using Blog.Persistance.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blog.Persistance;

public static class ServiceExtentions
{
    public static void ConfigureServices(this IServiceCollection services, IConfigurationSection configuration)
    {
        services.Configure<MongoDbSettings>(options =>
        {
            configuration.GetSection("MongoDbSettings");
        });

        services.AddSingleton<IMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
    }
}

