﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adoroid.CarService.Persistence;

public static class CarServicePersistenceServiceCollection
{
    public static IServiceCollection AddCarServicePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CarServiceDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString"),
                m => m.MigrationsAssembly("Adoroid.CarService.API"));
        });

        return services;
    }
}
