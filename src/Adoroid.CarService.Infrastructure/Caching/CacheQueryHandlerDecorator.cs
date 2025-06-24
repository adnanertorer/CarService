using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Infrastructure.Caching;

public class CacheQueryHandlerDecorator<TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : ICacheableQuery<TResponse>, IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly ICacheService _cache;

    public CacheQueryHandlerDecorator(
        IRequestHandler<TRequest, TResponse> inner,
        ICacheService cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = request.GetCacheKey();
        var cached = await _cache.GetAsync<TResponse>(cacheKey);
        if (cached is not null)
            return cached;

        var result = await _inner.Handle(request, cancellationToken);

        if (result?.GetType().IsGenericType == true && result.GetType().GetGenericTypeDefinition() == typeof(Response<>))
        {
            var dataProperty = result.GetType().GetProperty("Data");
            var data = dataProperty?.GetValue(result);

            if (data is not null)
            {
                await _cache.SetAsync(request.GetCacheKey(), data, request.Expiration);
            }
        }

        return result;
    }
}


