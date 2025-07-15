using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Application.Features.UserToCompanies.ExceptionMessages;
using Adoroid.CarService.Application.Features.UserToCompanies.MappingExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Commands.Create;

public record CreateUserToCompanyCommand(Guid CompanyId, int CompanyUserType) 
    : IRequest<Response<UserToCompanyDto>>;

public class CreateUserToCompanyCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<CreateUserToCompanyCommand, Response<UserToCompanyDto>>
{
    public async Task<Response<UserToCompanyDto>> Handle(CreateUserToCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyExist = await unitOfWork.Companies.IsCompanyExistsAsync(request.CompanyId, cancellationToken);
        if (!companyExist)
            return Response<UserToCompanyDto>.Fail(BusinessExceptionMessages.CompanyNotFound);

        var isExist = await unitOfWork.UserToCompanies.IsExists(Guid.Parse(currentUser.Id!), request.CompanyId, cancellationToken);

        if (isExist)
            return Response<UserToCompanyDto>.Fail(BusinessExceptionMessages.AlreadyExists);

        var entity = new UserToCompany
        {
            CompanyId = request.CompanyId,
            IsDeleted = false,
            UserId = Guid.Parse(currentUser.Id!),
            CreatedBy = Guid.Parse(currentUser.Id!),
            CreatedDate = DateTime.UtcNow,
            UserType = request.CompanyUserType
        };

        await unitOfWork.UserToCompanies.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<UserToCompanyDto>.Success(entity.FromEntity());
    }
}

