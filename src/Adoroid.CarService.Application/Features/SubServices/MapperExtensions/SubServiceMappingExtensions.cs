using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.SubServices.MapperExtensions;

public static class SubServiceMappingExtensions
{
    public static EmployeeDto EmployeeFromEntity(this Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name,
            Surname = employee.Surname
        };
    }

    public static MainServiceDto MainServiceFromEntity(this MainService mainService) {
        return new MainServiceDto
        {
            Cost = mainService.Cost,
            Description = mainService.Description,
            Id = mainService.Id,
            ServiceDate = mainService.ServiceDate,
            VehicleId = mainService.VehicleId,
            MainServiceStatus = mainService.ServiceStatus,
        };
    }

    public static SupplierDto SupplierFromEntity(this Supplier supplier) {
        return new SupplierDto
        {
            ContactName = supplier.ContactName,
            Id = supplier.Id,
            Name = supplier.Name,
            PhoneNumber = supplier.PhoneNumber
        };
    }

    public static SubServiceDto FromEntity(this SubService subService) {
        return new SubServiceDto
        {
            Cost = subService.Cost,
            Description = subService.Description,
            Discount = subService.Discount,
            Employee = subService.Employee?.EmployeeFromEntity(),
            EmployeeId = subService.EmployeeId,
            Id = subService.Id,
            MainService = subService.MainService?.MainServiceFromEntity(),
            MainServiceId = subService.MainServiceId,
            Material = subService.Material,
            MaterialBrand = subService.MaterialBrand,
            Operation = subService.Operation,
            OperationDate = subService.OperationDate,
            Supplier = subService.Supplier?.SupplierFromEntity(),
            SupplierId = subService.SupplierId,
            MaterialCost = subService.MaterialCost
        };
    }

    public static MobileSubServiceDto FromEntityToMobile(this SubService subService)
    {
        return new MobileSubServiceDto
        {
            Cost = subService.Cost,
            Description = subService.Description,
            Discount = subService.Discount,
            Id = subService.Id,
            MainService = subService.MainService?.MainServiceFromEntity(),
            MainServiceId = subService.MainServiceId,
            Material = subService.Material,
            MaterialBrand = subService.MaterialBrand,
            Operation = subService.Operation,
            OperationDate = subService.OperationDate,
        };
    }
}
