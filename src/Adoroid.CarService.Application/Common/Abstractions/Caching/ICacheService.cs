namespace Adoroid.CarService.Application.Common.Abstractions.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<string?> GetStringAsync(string key);
    Task SetStringAsync(string key, string value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);

    Task HashSetAsync<T>(string key, string field, T value);
    Task<T?> HashGetAsync<T>(string key, string field);
    Task<bool> HashFieldExistsAsync(string key, string field);
    Task HashRemoveAsync(string key, string field);
    Task<Dictionary<string, T>> HashGetAllAsync<T>(string key);

    Task PublishAsync(string channel, string message);
    void Subscribe(string channel, Action<string> handler);
}
