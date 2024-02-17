using Blog.Application.Interfaces;
using Blog.Persistance.Common;
using Blog.Persistance.Repositories;
using Microsoft.Extensions.Options;

namespace Blog.Persistance;

public static class ServiceExtentions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

        services.AddControllers();
    }
}

