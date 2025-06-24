namespace Adoroid.CarService.Application.Common.Abstractions.Caching;

public interface ICacheableQuery<TResponse>
{
    string GetCacheKey();
    TimeSpan? Expiration { get; }
}
