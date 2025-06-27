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
            Customer = vehicle.Customer != null ? new CustomerDto
            {
                Id = vehicle.CustomerId!.Value,
                Name = vehicle.Customer!.Name,
                Surname = vehicle.Customer!.Surname,
            } : null,
            Engine = vehicle.Engine,
            FuelTypeId = vehicle.FuelTypeId,
            Id = vehicle.Id,
            Model = vehicle.Model,
            Plate = vehicle.Plate,
            SerialNumber = vehicle.SerialNumber,
            Year = vehicle.Year,
            MainServices = vehicle.ListFromEntity(),
            MobileUserId = vehicle.MobileUserId,
            MobileUser = vehicle.MobileUser != null ? new MobileUserDto
            {
                Id = vehicle.MobileUserId!.Value,
                Name = vehicle.MobileUser!.Name,
                Surname = vehicle.MobileUser!.Surname,
                Email = vehicle.MobileUser!.Email,
                PhoneNumber = vehicle.MobileUser!.PhoneNumber
            } : null
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
