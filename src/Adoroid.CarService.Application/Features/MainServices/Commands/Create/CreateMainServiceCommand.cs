using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Enums;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.MainServices.Dtos;
using Adoroid.CarService.Application.Features.MainServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MainServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MainServices.Commands.Create;

public record CreateMainServiceCommand(Guid VehicleId, decimal Kilometer, DateTime ServiceDate, string? Description) : IRequest<Response<MainServiceDto>>;

public class CreateMainServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ILogger<CreateMainServiceCommandHandler> logger) 
    : IRequestHandler<CreateMainServiceCommand, Response<MainServiceDto>>
{
    public async Task<Response<MainServiceDto>> Handle(CreateMainServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var vehicle = await unitOfWork.Vehicles.GetByIdAsync(request.VehicleId, true, cancellationToken);

        if (vehicle is null)
            return Response<MainServiceDto>.Fail(BusinessExceptionMessages.VehicleNotFound);

        var entity = new MainService
        {
            Cost = 0,
            MaterialCost = 0,
            CreatedBy = Guid.Parse(currentUser.Id!),
            Description = request.Description,
            CreatedDate = DateTime.UtcNow,
            ServiceDate = request.ServiceDate,
            VehicleId = request.VehicleId,
            ServiceStatus = (int)MainServiceStatusEnum.Opened,
            CompanyId = companyId,
            Kilometers = request.Kilometer
        };

        var result = await unitOfWork.MainServices.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("New main service created with id {Id} for vehicle {VehicleId} by user {UserId}", result.Id, vehicle.Id, currentUser.Id);

        result.Vehicle = vehicle;

        var resultDto = result.FromEntity();

        return Response<MainServiceDto>.Success(resultDto);
    }
}