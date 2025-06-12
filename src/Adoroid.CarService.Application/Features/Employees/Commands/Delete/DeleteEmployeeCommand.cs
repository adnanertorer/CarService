using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Employees.ExecptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Employees.Commands.Delete;

public record DeleteEmployeeCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteEmployeeCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<DeleteEmployeeCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (employee is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        employee.DeletedDate = DateTime.UtcNow;
        employee.DeletedBy = Guid.Parse(currentUser.Id!);
        employee.IsDeleted = true;

        dbContext.Employees.Update(employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(employee.Id);
    }
}
