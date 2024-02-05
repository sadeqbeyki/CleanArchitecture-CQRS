using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Application.Helper;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache, string cacheKey, T dataValue, IConfiguration configuration)
    {
        //var cacheSettings = configuration.GetSection("CacheSettings");
        //DistributedCacheEntryOptions options = new()
        //{
        //    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(double.Parse(cacheSettings["AbsoluteExpireTimeSeconds"])),
        //    SlidingExpiration = TimeSpan.FromSeconds(double.Parse(cacheSettings["SlidingExpirationSeconds"]))
        //};

        var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddSeconds(double.Parse(configuration["CacheSettings:AbsoluteExpireTimeSeconds"])))
                .SetSlidingExpiration(TimeSpan.FromSeconds(double.Parse(configuration["CacheSettings:SlidingExpirationSeconds"])));

        var jsonData = JsonSerializer.Serialize(dataValue);
        await cache.SetStringAsync(
            cacheKey,
            jsonData,
            cacheOptions);
    }
    public static async Task<T> GetRecordAsync<T>(this IDistributedCache distributedCache, string cacheKey)
    {
        var jsonData = await distributedCache.GetStringAsync(cacheKey);
        return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
    }
}
