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
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Create;

public record CreateMainServiceCommand(Guid VehicleId, DateTime ServiceDate, string? Description) : IRequest<Response<MainServiceDto>>;


public class CreateMainServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser, 
    ICacheService cacheService, ILogger<CreateMainServiceCommandHandler> logger) 
    : IRequestHandler<CreateMainServiceCommand, Response<MainServiceDto>>
{
    const string redisKeyPrefix = "mainservice:list";
    public async Task<Response<MainServiceDto>> Handle(CreateMainServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var vehicle = await dbContext.Vehicles.AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == request.VehicleId, cancellationToken);

        if (vehicle is null)
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.VehicleNotFound);

        var entity = new MainService
        {
            Cost = 0,
            CreatedBy = Guid.Parse(currentUser.Id!),
            Description = request.Description,
            CreatedDate = DateTime.UtcNow,
            ServiceDate = request.ServiceDate,
            VehicleId = request.VehicleId,
            ServiceStatus = (int)MainServiceStatusEnum.Opened,
            CompanyId = companyId
        };

        var result = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        result.Entity.Vehicle = vehicle;

        var resultDto = result.Entity.FromEntity();

        try
        {
            await cacheService.AppendToListAsync($"{redisKeyPrefix}:{companyId}", resultDto, null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while appending to cache for main service creation.");
        }

        return Response<MainServiceDto>.Success(resultDto);
    }
}