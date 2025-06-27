using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Features.MainServices.Commands.Create;
using Adoroid.CarService.Infrastructure.Auth;
using Adoroid.CarService.Infrastructure.Auth.MobileUser;
using Adoroid.CarService.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediatR.Core;
using StackExchange.Redis;

namespace Adoroid.CarService.Infrastructure;

public static class CarServiceInsfrastructureServiceCollection
{
    public static IServiceCollection AddCarServiceInsfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAesEncryptionHelper, AesEncryptionHelper>();
        services.AddScoped<ITokenHandler, TokenHandler>();
        services.AddScoped<IMobileUserTokenHandler, MobileUserTokenHandler>();

        services.Scan(scan => scan.FromAssemblies(
            typeof(CreateMainServiceCommand).Assembly
        ).AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        services.AddSingleton<IConnectionMultiplexer>(sp =>
           ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnectionString")!));
        
        services.Decorate(typeof(IRequestHandler<,>), typeof(CacheQueryHandlerDecorator<,>));

        services.Decorate(typeof(IRequestHandler<,>), typeof(CacheRemoveCommandDecorator<,>));

        services.AddScoped<ICacheService, RedisCacheService>();
        return services;
    }
}
