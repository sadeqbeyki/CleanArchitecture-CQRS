using Blog.Application.Interfaces;
using Blog.Persistance.Common;
using Microsoft.Extensions.Options;

namespace Blog.Persistance;

public static class ServiceExtention
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

        services.AddControllers();
    }
}

