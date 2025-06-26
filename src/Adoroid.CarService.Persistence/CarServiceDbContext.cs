using Adoroid.CarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Adoroid.CarService.Persistence;

public class CarServiceDbContext(DbContextOptions<CarServiceDbContext> options): DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<MainService> MainServices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SubService> SubServices { get; set; }
    public DbSet<AccountingTransaction> AccountingTransactions { get; set; }
    public DbSet<MasterService> MasterServices { get; set; }
    public DbSet<CompanyService> CompanyServices { get; set; }
    public DbSet<MobileUser> MobileUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());
    }
}
