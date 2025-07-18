﻿using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.MainServices.MapperExtensions;

public static class MainServiceMappingExtension
{
    public static MainServiceDto FromEntity(this MainService mainService)
    {
        return new MainServiceDto
        {
            Cost = mainService.Cost,
            Description = mainService.Description,
            Id = mainService.Id,
            ServiceDate = mainService.ServiceDate,
            Vehicle = mainService.Vehicle !=null ? new VehicleDto
            {
                Brand = mainService.Vehicle!.Brand,
                Engine = mainService.Vehicle!.Engine,
                FuelTypeId = mainService.Vehicle!.FuelTypeId,
                Id = mainService.Vehicle!.Id,
                Model = mainService.Vehicle!.Model,
                Plate = mainService.Vehicle!.Plate,
                SerialNumber = mainService.Vehicle!.SerialNumber,
                Year = mainService.Vehicle!.Year
            } : null,
            VehicleId = mainService.VehicleId,
            MainServiceStatus = (MainServiceStatusEnum)mainService.ServiceStatus
        };
    }
}
