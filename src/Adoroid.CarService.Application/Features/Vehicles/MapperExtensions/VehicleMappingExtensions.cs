﻿using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;

public static class VehicleMappingExtensions
{
    public static VehicleDto FromEntity(this Vehicle vehicle)
    {
        return new VehicleDto
        {
            Brand = vehicle.Brand,
            Engine = vehicle.Engine,
            FuelTypeId = vehicle.FuelTypeId,
            Id = vehicle.Id,
            Model = vehicle.Model,
            Plate = vehicle.Plate,
            SerialNumber = vehicle.SerialNumber,
            Year = vehicle.Year
        };
    }

    public static MainServiceDto FromMainServiceEntity(this MainService mainService) {
        return new MainServiceDto
        {
            Cost = mainService.Cost,
            Description = mainService.Description,
            Id = mainService.Id,
            ServiceDate = mainService.ServiceDate,
            Company = mainService.Company?.FromCompanyEntity()
        };
    }

    public static CompanyDto FromCompanyEntity(this Company company)
    {
        return new CompanyDto
        {
            AuthorizedName = company.AuthorizedName,
            AuthorizedSurname = company.AuthorizedSurname,
            CompanyAddress = company.CompanyAddress,
            CompanyEmail = company.CompanyEmail,
            CompanyName = company.CompanyName,
            CompanyPhone = company.CompanyPhone,
            Id = company.Id
        };
    }

    public static List<MainServiceDto>? ListFromEntity(this Vehicle vehicle)
    {
        var list = new List<MainServiceDto>();
        if (vehicle.MainServices != null)
        {
            foreach (var mainService in vehicle.MainServices)
            {
                list.Add(mainService.FromMainServiceEntity());
            }
        }
        return list;
    }
}
