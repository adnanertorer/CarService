using Adoroid.CarService.Application.Features.MasterServices.Dtos;
using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Features.MasterServices.MapperExtensions;

public static class MasterServiceMappingExtensions
{
    public static MasterServiceDto FromEntity(this MasterService masterService)
    {
        return new MasterServiceDto
        {
            Id = masterService.Id,
            OrderIndex = masterService.OrderIndex,
            ServiceName = masterService.ServiceName
        };
    }
}
