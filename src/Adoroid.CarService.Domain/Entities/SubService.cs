using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class SubService : Entity<Guid>
{
    public Guid MainServiceId { get; set; }
    public string Operation { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime OperationDate { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public string? MaterialBrand { get; set; }
    public Guid? SupplierId { get; set; }
    public decimal? Discount { get; set; }
    public decimal Cost { get; set; }

    public MainService MainService { get; set; }
    public Employee Employee { get; set; }  
    public Supplier? Supplier { get; set; }
}
