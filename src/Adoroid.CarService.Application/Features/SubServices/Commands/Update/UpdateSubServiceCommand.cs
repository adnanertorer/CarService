using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Update;

public record UpdateSubServiceCommand(Guid Id, string Operation, Guid EmployeeId, DateTime OperationDate, string? Description,
    string? Material, string? MaterialBrand, Guid? SupplierId, decimal? Discount, decimal Cost) : IRequest<Response<SubServiceDto>>;

public class UpdateSubServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ICacheService cacheService)
    : IRequestHandler<UpdateSubServiceCommand, Response<SubServiceDto>>
{
    const string redisKeyPrefix = "subservice:list";
    public async Task<Response<SubServiceDto>> Handle(UpdateSubServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var entity = await dbContext.SubServices.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        var mainServiceEntity = await dbContext.MainServices.AsNoTracking().FirstOrDefaultAsync(i => i.Id == entity.MainServiceId, cancellationToken);
        if (mainServiceEntity == null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.MainServiceNotFound);

        var employee = await dbContext.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.EmployeeId, cancellationToken);

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

        entity.UpdatedBy = Guid.Parse(currentUser.Id!);
        entity.UpdatedDate = DateTime.UtcNow;

        dbContext.SubServices.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        entity.MainService = mainServiceEntity;
        entity.Employee = employee;

        var resultDto = entity.FromEntity();

        await cacheService.UpdateToListAsync($"{redisKeyPrefix}:{companyId}", request.Id.ToString(), resultDto, null);

        return Response<SubServiceDto>.Success(entity.FromEntity());
    }
}

