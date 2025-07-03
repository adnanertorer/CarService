using Adoroid.CarService.Application.Features.Customers.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Customers.MapperExtensions;

public static class CustomerMappingExtension
{
    public static CustomerDto FromEntity(this Customer customer)
    {
        return new CustomerDto
        {
            Address = customer.Address,
            CompanyId = customer.CompanyId,
            Email = customer.Email,
            Id = customer.Id,
            IsActive = customer.IsActive,
            Name = customer.Name,
            Phone = customer.Phone,
            Surname = customer.Surname,
            TaxNumber = customer.TaxNumber,
            TaxOffice = customer.TaxOffice,
            VehicleDtos = customer.ListFromEntity()
        };
    }

    public static List<VehicleDto>? ListFromEntity(this Customer customer)
    {
        var list = new List<VehicleDto>();
        if (customer.Vehicles != null)
        {
            foreach (var vehicle in customer.Vehicles)
            {
                list.Add(vehicle.VehicleFromEntity());
            }
        }
        return list;
    }

    public static VehicleDto VehicleFromEntity(this Vehicle vehicle)
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
}

