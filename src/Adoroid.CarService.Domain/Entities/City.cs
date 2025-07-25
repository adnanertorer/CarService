﻿namespace Adoroid.CarService.Domain.Entities;

public class City
{
    public City()
    {
        Companies = new HashSet<Company>();
        MobileUsers = new HashSet<MobileUser>();
        Suppliers = new HashSet<Supplier>();
        Customers = new HashSet<Customer>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int PlateNumber { get; set; }
    public int PhoneCode { get; set; }
    public int RowNumber { get; set; }

    public ICollection<Company>? Companies { get; set; }
    public ICollection<MobileUser>? MobileUsers { get; set; }
    public ICollection<Supplier>? Suppliers { get; set; }
    public ICollection<Customer>? Customers { get; set; }
}
