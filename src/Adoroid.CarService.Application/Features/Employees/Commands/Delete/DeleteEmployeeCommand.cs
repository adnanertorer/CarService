using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Employees.ExecptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Employees.Commands.Delete;

public record DeleteEmployeeCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<DeleteEmployeeCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await unitOfWork.Employees.GetByIdAsync(request.Id, false, cancellationToken);

        if (employee is null)
            return Response<Guid>.Fail(BusinessExceptionMessages.NotFound);

        employee.DeletedDate = DateTime.UtcNow;
        employee.DeletedBy = Guid.Parse(currentUser.Id!);
        employee.IsDeleted = true;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(employee.Id);
    }
}
