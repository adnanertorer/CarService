using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.CompanyMasterServices.Dtos;
using Adoroid.CarService.Application.Features.CompanyMasterServices.ExceptionMessages;
using Adoroid.CarService.Application.Features.CompanyMasterServices.MapperExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.CompanyMasterServices.Commands.Create;


public record CreateCompanyServiceCommand(Guid MasterServiceId) 
    : IRequest<Response<CompanyServiceDto>>;

public class CreateCompanyServiceCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<CreateCompanyServiceCommand, Response<CompanyServiceDto>>
{
    public async Task<Response<CompanyServiceDto>> Handle(CreateCompanyServiceCommand request, CancellationToken cancellationToken)
    {
        var companyId = currentUser.ValidCompanyId();

        var isExist = await unitOfWork.CompanyServices.IsExistAsync(companyId, request.MasterServiceId, cancellationToken);

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

        await unitOfWork.CompanyServices.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<CompanyServiceDto>.Success(entity.FromEntity());
    }
}

