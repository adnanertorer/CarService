using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Adoroid.CarService.Persistence;

public class CarServiceDbContextFactory : IDesignTimeDbContextFactory<CarServiceDbContext>
{
    public CarServiceDbContext CreateDbContext(string[] args = null)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Adoroid.CarService.Api");

        var configuration = new ConfigurationBuilder()
              .SetBasePath(basePath)
              .AddJsonFile("appsettings.json")
              .AddJsonFile($"appsettings.{environment}.json", optional: true)
              .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CarServiceDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString"));

        return new CarServiceDbContext(optionsBuilder.Options);
    }
}
