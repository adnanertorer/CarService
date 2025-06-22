using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Infrastructure.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Adoroid.CarService.Infrastructure;

public static class CarServiceInsfrastructureServiceCollection
{
    public static IServiceCollection AddCarServiceInsfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAesEncryptionHelper, AesEncryptionHelper>();
        services.AddScoped<ITokenHandler, TokenHandler>();
        return services;
    }
}
