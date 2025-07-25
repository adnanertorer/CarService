﻿using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Companies.Commands.Create;
using Adoroid.CarService.Application.Features.Companies.Commands.Create.Validators;
using Adoroid.Core.Application.Pipelines.Validation;
using Adoroid.Core.Application.Rules;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalMediatR.Behaviors;
using MinimalMediatR.Core;
using MinimalMediatR.Extensions;
using System.Reflection;

namespace Adoroid.CarService.Application;

public static class CarServiceApplicationServiceCollection
{
    public static IServiceCollection AddCarServiceApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), type: typeof(BaseBusinessRule));
        services.AddMinimalMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

    private static IServiceCollection AddSubClassesOfType(this IServiceCollection services, Assembly assembly, Type type,
    Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, type);
        return services;
    }
}
