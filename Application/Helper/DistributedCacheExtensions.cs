using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Application.Helper
{
    public static class DistributedCacheExtension
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recodeId, T data, IConfiguration configuration)
        {
            var cacheSettings = configuration.GetSection("CacheSettings");
            DistributedCacheEntryOptions options = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(double.Parse(cacheSettings["AbsoluteExpireTimeSeconds"])),
                SlidingExpiration = TimeSpan.FromSeconds(double.Parse(cacheSettings["SlidingExpirationSeconds"]))
            };

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recodeId, jsonData, options);
        }
        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
        }

        //public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recodeId, T data,
        //    TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null)
        //{
        //    DistributedCacheEntryOptions options = new()
        //    {
        //        AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(40),
        //        SlidingExpiration = slidingExpirationTime ?? TimeSpan.FromSeconds(10)
        //    };

        //    var jsonData = JsonSerializer.Serialize(data);
        //    await cache.SetStringAsync(recodeId, jsonData, options);
        //}


    }
}
