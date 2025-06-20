﻿using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.SubServices.Dtos;
using Adoroid.CarService.Application.Features.SubServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.SubServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.SubServices.Commands.Create;

public record CreateSubServiceCommand(Guid MainServiceId, string Operation, Guid EmployeeId, DateTime OperationDate, string? Description,
    string? Material, string? MaterialBrand, Guid? SupplierId, decimal? Discount, decimal Cost) : IRequest<Response<SubServiceDto>>;

public class CreateSubServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<CreateSubServiceCommand, Response<SubServiceDto>>
{
    public async Task<Response<SubServiceDto>> Handle(CreateSubServiceCommand request, CancellationToken cancellationToken)
    {
        var mainServiceEntity = await dbContext.MainServices.FirstOrDefaultAsync(i => i.Id == request.MainServiceId, cancellationToken);

        if (mainServiceEntity == null)
            return Response<SubServiceDto>.Fail(BusinessExceptionMessages.MainServiceNotFound);

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

        await dbContext.AddAsync(entity, cancellationToken);

        mainServiceEntity.Cost += request.Cost - (request.Discount ?? 0);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<SubServiceDto>.Success(entity.FromEntity());
    }
}
