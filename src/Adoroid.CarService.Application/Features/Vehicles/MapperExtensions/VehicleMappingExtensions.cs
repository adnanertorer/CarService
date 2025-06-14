using Adoroid.CarService.Application.Features.Vehicles.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Vehicles.MapperExtensions;

public static class VehicleMappingExtensions
{
    public static VehicleDto FromEntity(this Vehicle vehicle)
    {
        return new VehicleDto
        {
            Brand = vehicle.Brand,
            CustomerId = vehicle.CustomerId,
            Customer = new CustomerDto
            {
                Id = vehicle.CustomerId,
                Name = vehicle.Customer!.Name,
                Surname = vehicle.Customer!.Surname,
            },
            Engine = vehicle.Engine,
            FuelTypeId = vehicle.FuelTypeId,
            Id = vehicle.Id,
            Model = vehicle.Model,
            Plate = vehicle.Plate,
            SerilNumber = vehicle.SerilNumber,
            Year = vehicle.Year,
            MainServices = vehicle.ListFromEntity()
        };
    }

    public static MainServiceDto FromMainServiceEntity(this MainService mainService) {
        return new MainServiceDto
        {
            Cost = mainService.Cost,
            Description = mainService.Description,
            Id = mainService.Id,
            ServiceDate = mainService.ServiceDate
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
