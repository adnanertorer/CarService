using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Application.Features.CompanyMasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.CompanyMasterServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Create;


public record CreateCompanyServiceCommand(Guid MasterServiceId) 
    : IRequest<Response<CompanyServiceDto>>;

public class CreateCompanyServiceCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<CreateCompanyServiceCommand, Response<CompanyServiceDto>>
{
    public async Task<Response<CompanyServiceDto>> Handle(CreateCompanyServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var isExist = await dbContext.CompanyServices
            .AsNoTracking()
            .AnyAsync(i => i.CompanyId == companyId && i.MasterServiceId == request.MasterServiceId, cancellationToken);

        if (isExist)
            return Response<CompanyServiceDto>.Fail(BusinessExceptionMessages.AlreadyExists);

        var entity = new CompanyService
        {
            CompanyId = companyId,
            IsDeleted = false,
            MasterServiceId = request.MasterServiceId,
            CreatedBy = Guid.Parse(currentUser.Id!),
            CreatedDate = DateTime.UtcNow
        };

        await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<CompanyServiceDto>.Success(entity.FromEntity());
    }
}

