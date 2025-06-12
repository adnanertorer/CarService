using Adoroid.CarService.Application.Features.Employees.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.Employees.MapperExtensions;

public static class EmployeeMappingExtension
{
    public static EmployeeDto FromEntity(this Employee employee)
    {
        return new EmployeeDto
        {
            Address = employee.Address,
            CompanyId = employee.CompanyId,
            Email = employee.Email,
            Id = employee.Id,
            IsActive = employee.IsActive,
            Name = employee.Name,
            PhoneNumber = employee.PhoneNumber,
            Surname = employee.Surname
        };
    }

    public static Employee FromModel(this EmployeeDto employee) 
    {
        return new Employee
        {
            Address = employee.Address,
            CompanyId = employee.CompanyId,
            Email = employee.Email,
            Id = employee.Id,
            IsActive = employee.IsActive,
            Name = employee.Name,
            PhoneNumber = employee.PhoneNumber,
            Surname = employee.Surname
        };
    }
}
