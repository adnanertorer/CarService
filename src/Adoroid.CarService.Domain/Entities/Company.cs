﻿using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class Company : Entity<Guid>
{
    public Company()
    {
        Customers = new HashSet<Customer>();
        Employees = new HashSet<Employee>();
        Suppliers = new HashSet<Supplier>();
        AccountingTransactions = new HashSet<AccountingTransaction>();
        CompanyServices = new HashSet<CompanyService>();
        MainServices = new HashSet<MainService>();
        UserToCompanies = new HashSet<UserToCompany>();
    }

    public string CompanyName { get; set; }
    public string AuthorizedName { get; set; }
    public string AuthorizedSurname { get; set; }
    public string TaxNumber { get; set; }
    public string TaxOffice { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public string CompanyAddress { get; set; }
    public string CompanyPhone { get; set; }
    public string CompanyEmail { get; set; }

    public ICollection<Customer>? Customers { get; set; }
    public ICollection<Employee>? Employees { get; set; }
    public ICollection<Supplier>? Suppliers { get; set; }
    public ICollection<AccountingTransaction> AccountingTransactions { get; set; }
    public ICollection<CompanyService> CompanyServices { get; set; }
    public ICollection<MainService> MainServices { get; set; }
    public ICollection<UserToCompany>? UserToCompanies { get; set; }

    public City? City { get; set; }
    public District? District { get; set; }
}
