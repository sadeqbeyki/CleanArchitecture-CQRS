using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Identity.Application.Helper
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T?> GetObjectAsync<T>(this IDistributedCache distributedCache, string key,
             CancellationToken cancellationToken = default) where T : class
        {
            string? cachedData = await distributedCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
                return null;

            return JsonConvert.DeserializeObject<T>(cachedData);
        }

        public static async Task SetObjectAsync<T>(this IDistributedCache distributedCache,
            string key, T data, IConfiguration configuration, CancellationToken cancellationToken = default) where T : class
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(double.Parse(configuration["CacheSettings:AbsoluteExpireTimeSeconds"]))
                //SlidingExpiration = TimeSpan.FromMinutes(10)
            };
            await distributedCache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(data),
                cacheOptions,
                cancellationToken);
        }
    }
}
