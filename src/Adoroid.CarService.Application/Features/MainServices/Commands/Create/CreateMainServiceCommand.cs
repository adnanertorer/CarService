using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Abstractions.Caching;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Create;

public record CreateMainServiceCommand(Guid VehicleId, DateTime ServiceDate, string? Description) : IRequest<Response<MainServiceDto>>;


public class CreateMainServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, ICacheService cacheService) 
    : IRequestHandler<CreateMainServiceCommand, Response<MainServiceDto>>
{
    public async Task<Response<MainServiceDto>> Handle(CreateMainServiceCommand request, CancellationToken cancellationToken)
    {
        var isExist = await dbContext.MainServices
            .AsNoTracking()
            .Where(i => i.VehicleId == request.VehicleId)
            .WhereDateIsBetween(i => i.ServiceDate, request.ServiceDate)
            .AnyAsync(cancellationToken);

        if (isExist)
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.AlreadyExists);

        var entity = new MainService
        {
            Cost = 0,
            CreatedBy = Guid.Parse(currentUser.Id!),
            Description = request.Description,
            CreatedDate = DateTime.UtcNow,
            ServiceDate = request.ServiceDate,
            VehicleId = request.VehicleId,
            ServiceStatus = (int)MainServiceStatusEnum.Opened
        };

        var result = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cacheService.SetAsync($"main-service:{entity.Id}", entity, null);

        return Response<MainServiceDto>.Success(result.Entity.FromEntity());
    }
}