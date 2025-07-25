﻿namespace Adoroid.CarService.Application.Features.Customers.Dtos;

public class CustomerDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Email { get; set; }
    public string Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public string? TaxNumber { get; set; }
    public string? TaxOffice { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }

    public CityModel? City { get; set; }
    public DistrictModel? District { get; set; }
}
