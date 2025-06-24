using Adoroid.CarService.Application.Common.Abstractions.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace Adoroid.CarService.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value!) : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    public async Task RemoveAsync(string key) => await _db.KeyDeleteAsync(key);

    public async Task<bool> ExistsAsync(string key) => await _db.KeyExistsAsync(key);

    public async Task HashSetAsync<T>(string key, string field, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.HashSetAsync(key, new HashEntry[] { new(field, json) });
    }

    public async Task<T?> HashGetAsync<T>(string key, string field)
    {
        var value = await _db.HashGetAsync(key, field);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value!) : default;
    }

    public async Task<bool> HashFieldExistsAsync(string key, string field)
        => await _db.HashExistsAsync(key, field);

    public async Task HashRemoveAsync(string key, string field)
        => await _db.HashDeleteAsync(key, field);

    public async Task<Dictionary<string, T>> HashGetAllAsync<T>(string key)
    {
        var entries = await _db.HashGetAllAsync(key);
        return entries.ToDictionary(
            e => e.Name.ToString(),
            e => JsonSerializer.Deserialize<T>(e.Value!)!
        );
    }

    public async Task PublishAsync(string channel, string message)
    {
        var sub = _redis.GetSubscriber();
        await sub.PublishAsync(channel, message);
    }

    public void Subscribe(string channel, Action<string> handler)
    {
        var sub = _redis.GetSubscriber();
        sub.Subscribe(channel, (ch, msg) => handler(msg!));
    }
}
