using Adoroid.CarService.Application.Common.Abstractions.Caching;

namespace Adoroid.CarService.Application.Common.Extensions;

public static class CacheServiceExtensions
{
    public static async Task<T> GetOrSetAsync<T>(this ICacheService cache, string key, Func<Task<T>> factory, TimeSpan? expiry = null) where T: class
    {
        var cached = await cache.GetAsync<T>(key);
        if (cached is not null)
            return cached;

        var data = await factory();
        await cache.SetAsync(key, data, expiry);
        return data;
    }
}
