﻿using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Employees.Dtos;
using Adoroid.CarService.Application.Features.Employees.ExecptionMessages;
using Adoroid.CarService.Application.Features.Employees.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Employees.Commands.Create;

public record CreateEmployeeCommand(string Name, string Surname, string PhoneNumber, bool IsActive, string? Email, string? Address) 
    : IRequest<Response<EmployeeDto>>;

public class CreateEmployeeCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<CreateEmployeeCommand, Response<EmployeeDto>>
{
    public async Task<Response<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var isExist = await dbContext.Employees
             .AsNoTracking()
             .AnyAsync(i => i.Name == request.Name && i.Surname == request.Surname, cancellationToken);

        if (isExist)
            return Response<EmployeeDto>.Fail(BusinessExceptionMessages.AlreadyExists);

        var employeeEntity = new Employee
        {
            Address = request.Address,
            CompanyId = companyId,
            CreatedBy = Guid.Parse(currentUser.Id!),
            Email = request.Email,
            IsActive = request.IsActive,
            IsDeleted = false,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Surname = request.Surname,
            CreatedDate = DateTime.UtcNow
        };

        var result = await dbContext.Employees.AddAsync(employeeEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<EmployeeDto>.Success(result.Entity.FromEntity());
    }
}
