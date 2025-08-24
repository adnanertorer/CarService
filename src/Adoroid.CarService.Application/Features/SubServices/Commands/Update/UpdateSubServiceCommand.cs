using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Update;

public record UpdateSubServiceCommand(Guid Id, string Operation, Guid EmployeeId, DateTime OperationDate, string? Description,
    string? Material, string? MaterialBrand, decimal? MaterialCost, Guid? SupplierId, decimal? Discount, decimal Cost) : IRequest<Response<SubServiceDto>>;

public class UpdateSubServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ILogger<UpdateSubServiceCommandHandler> logger)
    : IRequestHandler<UpdateSubServiceCommand, Response<SubServiceDto>>
{
    
    public async Task<Response<SubServiceDto>> Handle(UpdateSubServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var entity = await unitOfWork.SubServices.GetByIdAsync(request.Id, false, cancellationToken);

        if (entity is null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        var mainServiceEntity = await unitOfWork.MainServices.GetByIdAsync(entity.MainServiceId, true, cancellationToken);
        if (mainServiceEntity == null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.MainServiceNotFound);

        var employee = await unitOfWork.Employees.GetByIdAsync(request.EmployeeId, true, cancellationToken);

        if (employee == null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.EmployeeNotFound);

        entity.SupplierId = request.SupplierId;
        entity.Description = request.Description;
        entity.Material = request.Material;
        entity.MaterialBrand = request.MaterialBrand;
        entity.Cost = request.Cost;
        entity.Discount = request.Discount;
        entity.EmployeeId = request.EmployeeId;
        entity.Operation = request.Operation;
        entity.OperationDate = request.OperationDate;
        entity.MaterialCost = request.MaterialCost;

        entity.UpdatedBy = Guid.Parse(currentUser.Id!);
        entity.UpdatedDate = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Sub service with id {Id} updated by user {UserId}", entity.Id, currentUser.Id);

        entity.MainService = mainServiceEntity;
        entity.Employee = employee;

        var resultDto = entity.FromEntity();

        return Response<SubServiceDto>.Success(entity.FromEntity());
    }
}

