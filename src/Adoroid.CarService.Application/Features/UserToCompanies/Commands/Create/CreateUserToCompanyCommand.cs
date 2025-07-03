using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Application.Features.UserToCompanies.ExceptionMessages;
using Adoroid.CarService.Application.Features.UserToCompanies.MappingExtensions;
using Adoroid.CarService.Domain.Entities;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Commands.Create;

public record CreateUserToCompanyCommand(Guid CompanyId, int CompanyUserType) 
    : IRequest<Response<UserToCompanyDto>>;

public class CreateUserToCompanyCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<CreateUserToCompanyCommand, Response<UserToCompanyDto>>
{
    public async Task<Response<UserToCompanyDto>> Handle(CreateUserToCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyExist = await dbContext.Companies
            .AsNoTracking()
            .AnyAsync(i => i.Id == request.CompanyId, cancellationToken);

        if (!companyExist)
            return Response<UserToCompanyDto>.Fail(BusinessExceptionMessages.CompanyNotFound);

        var isExist = await dbContext.UserToCompanies
            .AsNoTracking()
            .AnyAsync(i => i.UserId == Guid.Parse(currentUser.Id!) && i.CompanyId == request.CompanyId, cancellationToken);

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

        await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<UserToCompanyDto>.Success(entity.FromEntity());
    }
}

