using Adoroid.CarService.Application.Common.Abstractions.Caching;
using System.Text.Json;

namespace Adoroid.CarService.Application.Common.Extensions;

public static class CacheServiceExtensions
{
    public static async Task<T> GetOrSetListAsync<T>(this ICacheService cache, string key, Func<Task<T>> factory, TimeSpan? expiry = null) where T: class
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

    public static async Task AppendToListAsync<T>(this ICacheService cache, string listKey, T data, TimeSpan? expiry = null)
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

    public static async Task UpdateToListAsync<T>(this ICacheService cache, string listKey, string dataKey, T data, TimeSpan? expiry = null)
    {
        var list = await cache.GetAsync<List<JsonElement>>(listKey) ?? [];

        var itemToRemove = list.FirstOrDefault(x =>
            x.TryGetProperty("Id", out var idProp) && idProp.GetString() == dataKey);

        if (itemToRemove.ValueKind != JsonValueKind.Undefined)
        {
            list.Remove(itemToRemove);
        }

        var element = ConvertToJsonElement(data);
        list.Add(element);

        await cache.SetAsync(listKey, list, expiry);
    }

    public static async Task RemoveFromListAsync<T>(this ICacheService cache, string listKey, string dataKey)
    {
        var list = await cache.GetAsync<List<JsonElement>>(listKey);
        if (list is null) return;

        var itemToRemove = list.FirstOrDefault(x =>
            x.TryGetProperty("Id", out var idProp) && idProp.GetString() == dataKey);

        if (itemToRemove.ValueKind != JsonValueKind.Undefined)
        {
            list.Remove(itemToRemove);
            await cache.SetAsync(listKey, list);
        }
    }

    static JsonElement ConvertToJsonElement<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<JsonElement>(json);
    }
}
