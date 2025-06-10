using Adoroid.CarService.Application.Features.Suppliers.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Suppliers.MapperExtensions;

public static class SupplierMappingExtension
{
    public static SupplierDto FromEntity(this Supplier supplier)
    {
        return new SupplierDto
        {
            Address = supplier.Address,
            CompanyId = supplier.CompanyId,
            ContactName = supplier.ContactName,
            Email = supplier.Email,
            Id = supplier.Id,
            Name = supplier.Name,
            PhoneNumber = supplier.PhoneNumber,
        };
    }

    public static Supplier FromModel(this SupplierDto supplierDto)
    {
        return new Supplier
        {
            Address = supplierDto.Address,
            CompanyId = supplierDto.CompanyId,
            ContactName = supplierDto.ContactName,
            Email = supplierDto.Email,
            Name = supplierDto.Name,
            PhoneNumber = supplierDto.PhoneNumber
        };
    }
}
