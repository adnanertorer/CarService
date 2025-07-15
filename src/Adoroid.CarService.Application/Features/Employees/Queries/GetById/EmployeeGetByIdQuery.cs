using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Features.Employees.Dtos;
using Adoroid.CarService.Application.Features.Employees.ExecptionMessages;
using Adoroid.CarService.Application.Features.Employees.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Employees.Queries.GetById;

public record EmployeeGetByIdQuery(Guid Id) : IRequest<Response<EmployeeDto>>;

public class EmployeeGetByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<EmployeeGetByIdQuery, Response<EmployeeDto>>
{
    public async Task<Response<EmployeeDto>> Handle(EmployeeGetByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await unitOfWork.Employees.GetByIdAsync(request.Id, true, cancellationToken);

        if (employee is null)
            return Response<EmployeeDto>.Fail(BusinessExceptionMessages.NotFound);

        return Response<EmployeeDto>.Success(employee.FromEntity());
    }
}
