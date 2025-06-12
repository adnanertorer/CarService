using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Employees.Dtos;
using Adoroid.CarService.Application.Features.Employees.ExecptionMessages;
using Adoroid.CarService.Application.Features.Employees.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Employees.Commands.Update;

public record UpdateEmployeeCommand(Guid Id, string Name, string Surname, string PhoneNumber, bool IsActive, string? Email, string? Address)
    : IRequest<Response<EmployeeDto>>;

public class UpdateEmployeeCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<UpdateEmployeeCommand, Response<EmployeeDto>>
{
    public async Task<Response<EmployeeDto>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (employee is null)
            return Response<EmployeeDto>.Fail(BusinessExceptionMessages.NotFound);

        employee.UpdatedDate = DateTime.UtcNow;
        employee.UpdatedBy = Guid.Parse(currentUser.Id!);
        employee.Surname = request.Surname;
        employee.PhoneNumber = request.PhoneNumber;
        employee.Name = request.Name;
        employee.Name = request.Name;
        employee.IsActive = request.IsActive;
        employee.Email = request.Email;
        employee.Address = request.Address;

        dbContext.Employees.Update(employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<EmployeeDto>.Success(employee.FromEntity());
    }
}
