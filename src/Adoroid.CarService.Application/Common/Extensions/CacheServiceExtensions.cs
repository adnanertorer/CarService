using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;

namespace Adoroid.CarService.Application.Common.Extensions;

public static class CacheServiceExtensions
{
    public static async Task<T> GetOrSetAsync<T>(this ICacheService cache, string key, Func<Task<T>> factory, TimeSpan? expiry = null) where T: class
    {
        var cached = await cache.GetAsync<T>(key);
        if (cached is not null)
            return cached;

        var result = await factory();
        if (result?.GetType().IsGenericType == true && result.GetType().GetGenericTypeDefinition() == typeof(Paginate<>))
        {
            var dataProperty = result.GetType().GetProperty("Items");
            var data = dataProperty?.GetValue(result);

            if (data is not null)
            {
                await cache.SetAsync(key, data, expiry);
            }
        }
        return result;
    }
}
