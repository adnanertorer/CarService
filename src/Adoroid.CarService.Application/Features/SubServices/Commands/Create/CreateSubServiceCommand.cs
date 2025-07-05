using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MasterServices.Commands.Create;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Create;

public record CreateSubServiceCommand(Guid MainServiceId, string Operation, Guid EmployeeId, DateTime OperationDate, string? Description,
    string? Material, string? MaterialBrand, Guid? SupplierId, decimal? Discount, decimal Cost) : IRequest<Response<SubServiceDto>>;

public class CreateSubServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ICacheService cacheService, ILogger<CreateSubServiceCommandHandler> logger)
    : IRequestHandler<CreateSubServiceCommand, Response<SubServiceDto>>
{
    const string redisKeyPrefix = "subservice:list";
    public async Task<Response<SubServiceDto>> Handle(CreateSubServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var mainServiceEntity = await dbContext.MainServices.FirstOrDefaultAsync(i => i.Id == request.MainServiceId, cancellationToken);

        if (mainServiceEntity == null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.MainServiceNotFound);

        var employee = await dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.EmployeeId, cancellationToken);

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
            CreatedDate = DateTime.UtcNow
        };

        var result = await dbContext.AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var model = result.Entity;
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
