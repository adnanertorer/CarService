using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.Core.Application.Wrappers;
using Adoroid.Core.Repository.Paging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Adoroid.CarService.Application.Common.Extensions;

public static class CacheServiceExtensions
{
    public static async Task<T> GetOrSetPaginateAsync<T>(this ICacheService cache, string key, Func<Task<T>> factory, TimeSpan? expiry = null) where T: class
    {
        var cached = await cache.GetAsync<T>(key);
        if (cached is not null)
            return cached;

        var result = await factory();
        if(result is not null)
        {
            await cache.SetAsync(key, result, expiry);
        }
        return result;
    }

    public static async Task TryAppendToListAsync<T>(this ICacheService cache, string listKey, T data, TimeSpan? expiry = null)
    {
        var existingList = await cache.GetAsync<List<T>>(listKey);

        if (existingList is null)
        {
            await cache.SetAsync(listKey, new List<T> { data }, expiry);
        }
        else
        {
            existingList.Add(data);
            await cache.SetAsync(listKey, existingList, expiry);
        }
    }
}
