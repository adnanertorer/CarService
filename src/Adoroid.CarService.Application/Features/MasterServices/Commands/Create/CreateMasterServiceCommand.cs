using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.MasterServices.Dtos;
using Adoroid.CarService.Application.Features.MasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.MasterServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.MasterServices.Commands.Create;

public record CreateMasterServiceCommand(string ServiceName, int OrderIndex) : IRequest<Response<MasterServiceDto>>;


public class CreateMasterServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<CreateMasterServiceCommand, Response<MasterServiceDto>>
{
    public async Task<Response<MasterServiceDto>> Handle(CreateMasterServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceList = await dbContext.MasterServices
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var isExist = serviceList.Any(i => i.ServiceName == request.ServiceName);

        if (isExist)
            return Response<MasterServiceDto>.Fail(BusinessExceptionMessages.AlreadyExists);

        var isExistIndex = serviceList.Any(i => i.OrderIndex == request.OrderIndex);

        if (isExistIndex)
            return Response<MasterServiceDto>.Fail(BusinessExceptionMessages.IndexAlreadyExists);

        var entity = new MasterService
        {
            CreatedBy = Guid.Parse(currentUser.Id!),
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false,
            OrderIndex = request.OrderIndex,
            ServiceName = request.ServiceName
        };

        await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<MasterServiceDto>.Success(entity.FromEntity());
    }
}

