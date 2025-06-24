using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        await _cache.SetAsync(request.GetCacheKey(), result, request.Expiration);

        return result;
    }
}


