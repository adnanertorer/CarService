using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Application.Features.CompanyMasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.CompanyMasterServices.MapperExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Update;

public record UpdateCompanyServiceCommand(Guid Id, Guid MasterServiceId) : IRequest<Response<CompanyServiceDto>>;

public class UpdateCompanyServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<UpdateCompanyServiceCommand, Response<CompanyServiceDto>>
{
    public async Task<Response<CompanyServiceDto>> Handle(UpdateCompanyServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.CompanyServices.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<CompanyServiceDto>.Fail(BusinessExceptionMessages.NotFound);

        entity.MasterServiceId = request.MasterServiceId;
        entity.UpdatedBy = Guid.Parse(currentUser.Id!);
        entity.UpdatedDate = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<CompanyServiceDto>.Success(entity.FromEntity());
    }
}

