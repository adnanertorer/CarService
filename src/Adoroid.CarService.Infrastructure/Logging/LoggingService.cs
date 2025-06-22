using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Extensions.Hosting;
using Serilog.Sinks.Elasticsearch;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Adoroid.CarService.Infrastructure.Logging;

public static class LoggingService
{
    public static IServiceCollection AddLoggingAndMonitoring(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var username = builder.Configuration.GetSection("ElasticConfig")["Username"];
        var password = builder.Configuration.GetSection("ElasticConfig")["Password"];
        var url = builder.Configuration.GetSection("ElasticConfig")["Url"];
        var indexFormat = builder.Configuration.GetSection("ElasticConfig")["IndexFormat"];
        var resourceName = builder.Configuration.GetSection("ElasticConfig")["ResourceName"];


        if (username == null || password == null || url == null || indexFormat == null || resourceName == null)
             throw new InvalidOperationException("Elastic configuration section is missing or malformed.");

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(url!))
            {
                AutoRegisterTemplate = true,
                IndexFormat = indexFormat,
                FailureCallback = (logEvent, ex) => Console.WriteLine($"Elasticsearch error:{ex.Message}"),
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog | EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.ThrowException,
                ModifyConnectionSettings = conn => conn.BasicAuthentication(username, password).ServerCertificateValidationCallback((sender, cert, chain, errors) => true)
            }).CreateLogger();

        services.AddLogging(loggingBuilder => {
            loggingBuilder.AddSerilog();
        });

        builder.Host.UseSerilog(Log.Logger);
        services.AddSingleton<DiagnosticContext>();


        services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(resourceName!))
                .AddAspNetCoreInstrumentation()  //ASP.NET Core tracing
                .AddHttpClientInstrumentation() //HTTP isteklerinin takibi
            )
            .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
                .AddMeter(resourceName!) //Custom metrikler eklemek için
                .AddAspNetCoreInstrumentation() // ASP.NET Core metrikleri
                .AddRuntimeInstrumentation());// .NET Runtime metriklerini ekler

        return services;
    }
}
