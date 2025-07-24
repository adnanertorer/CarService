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
            CityId = customer.CityId,
            DistrictId = customer.DistrictId,
            City = customer.City != null ? new CityModel
            {
                Id = customer.City.Id,
                Name = customer.City.Name
            } : null,
            District = customer.District != null ? new DistrictModel
            {
                Id = customer.District.Id,
                Name = customer.District.Name
            } : null,
        };
    }

    public static VehicleUserDto VehicleUserFromEntity(this VehicleUser vehicleUser)
    {
        return new VehicleUserDto
        {
            Id = vehicleUser.Id,
            UserId = vehicleUser.UserId,
            UserTypeId = vehicleUser.UserTypeId,
            VehicleId = vehicleUser.VehicleId,
            Vehicle = vehicleUser.Vehicle.VehicleFromEntity()
        };
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

