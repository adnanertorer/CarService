using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Create;

public record CreateSubServiceCommand(Guid MainServiceId, string Operation, Guid EmployeeId, DateTime OperationDate, string? Description,
    string? Material, string? MaterialBrand, decimal? MaterialCost, Guid? SupplierId, decimal? Discount, decimal Cost) : IRequest<Response<SubServiceDto>>;

public class CreateSubServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICacheService cacheService, ILogger<CreateSubServiceCommandHandler> logger)
    : IRequestHandler<CreateSubServiceCommand, Response<SubServiceDto>>
{
 
    public async Task<Response<SubServiceDto>> Handle(CreateSubServiceCommand request, CancellationToken cancellationToken)
    {
        var redisKeyPrefix = $"subservice:list:{request.MainServiceId}";

        var companyId = currentUser.ValidCompanyId();

        var mainServiceEntity = await unitOfWork.MainServices.GetByIdAsync(request.MainServiceId, true, cancellationToken);

        if (mainServiceEntity == null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.MainServiceNotFound);

        var employee = await unitOfWork.Employees.GetByIdAsync(request.EmployeeId, true, cancellationToken);

        if (employee == null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.EmployeeNotFound);

        var entity = new SubService
        {
            Cost = request.Cost,
            Description = request.Description,
            Discount = request.Discount,
            EmployeeId = request.EmployeeId,
            IsDeleted = false,
            MainServiceId = request.MainServiceId,
            MaterialBrand = request.MaterialBrand,
            Material = request.Material,
            SupplierId = request.SupplierId,
            Operation = request.Operation,
            OperationDate = request.OperationDate,
            CreatedBy = Guid.Parse(currentUser.Id!),
            CreatedDate = DateTime.UtcNow,
            MaterialCost = request.MaterialCost
        };

        var result = await unitOfWork.SubServices.AddAsync(entity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var model = result;
        model.MainService = mainServiceEntity;
        model.Employee = employee;

        var resultDto = model.FromEntity();

        try
        {
            await cacheService.AppendToListAsync($"{redisKeyPrefix}:{companyId}", resultDto, null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while appending to cache for sub service creation.");
        }

        return Response<SubServiceDto>.Success(resultDto);
    }
}
