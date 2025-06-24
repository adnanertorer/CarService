using Adoroid.CarService.Application.Common.Abstractions.Caching;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Infrastructure.Caching;

public class CacheRemoveCommandDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly ICacheService _cache;

    public CacheRemoveCommandDecorator(
        IRequestHandler<TRequest, TResponse> inner,
        ICacheService cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var result = await _inner.Handle(request, cancellationToken);

        if (request is ICacheRemovableCommand removable)
        {
            foreach (var key in removable.GetCacheKeysToRemove())
            {
                await _cache.RemoveAsync(key);
            }
        }

        return result;
    }
}

