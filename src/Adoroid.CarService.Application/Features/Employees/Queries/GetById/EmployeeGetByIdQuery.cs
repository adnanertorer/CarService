using Adoroid.CarService.Application.Features.Employees.Dtos;
using Adoroid.CarService.Application.Features.Employees.ExecptionMessages;
using Adoroid.CarService.Application.Features.Employees.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Employees.Queries.GetById;

public record EmployeeGetByIdQuery(Guid Id) : IRequest<Response<EmployeeDto>>;

public class EmployeeGetByIdQueryHandler(CarServiceDbContext dbContext) : IRequestHandler<EmployeeGetByIdQuery, Response<EmployeeDto>>
{
    public async Task<Response<EmployeeDto>> Handle(EmployeeGetByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (employee is null)
            return Response<EmployeeDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<EmployeeDto>.Success(employee.FromEntity());
    }
}
