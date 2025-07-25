﻿using Adoroid.Core.Repository.Repositories;

namespace Adoroid.CarService.Domain.Entities;

public class Vehicle : Entity<Guid>
{
    public Vehicle()
    {
        MainServices = new HashSet<MainService>();
        VehicleUsers = new HashSet<VehicleUser>();
    }

    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Plate { get; set; }
    public string? Engine { get; set; }
    public int FuelTypeId { get; set; }
    public string? SerialNumber { get; set; }

    public ICollection<MainService> MainServices { get; set; }
    public ICollection<VehicleUser> VehicleUsers { get; set; }
}
