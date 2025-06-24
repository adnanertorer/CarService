namespace Adoroid.CarService.Application.Common.Abstractions.Caching;

public  interface ICacheRemovableCommand
{
    IEnumerable<string> GetCacheKeysToRemove();
}
