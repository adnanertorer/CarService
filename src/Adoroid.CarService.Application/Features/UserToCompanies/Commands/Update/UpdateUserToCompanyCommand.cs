using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.UserToCompanies.Dtos;
using Adoroid.CarService.Application.Features.UserToCompanies.ExceptionMessages;
using Adoroid.CarService.Application.Features.UserToCompanies.MappingExtensions;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.UserToCompanies.Commands.Update;

public record UpdateUserToCompanyCommand(Guid Id, Guid CompanyId, int CompanyUserType) : IRequest<Response<UserToCompanyDto>>;

public class UpdateUserToCompanyCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<UpdateUserToCompanyCommand, Response<UserToCompanyDto>>
{
    public async Task<Response<UserToCompanyDto>> Handle(UpdateUserToCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.UserToCompanies.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity is null)
            return Response<UserToCompanyDto>.Fail(BusinessExceptionMessages.NotFound);

        
        entity.CompanyId = request.CompanyId;
        entity.UserType = request.CompanyUserType;
        entity.UserId = Guid.Parse(currentUser.Id!);
        entity.UpdatedBy = Guid.Parse(currentUser.Id!);
        entity.UpdatedDate = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<UserToCompanyDto>.Success(entity.FromEntity());
    }
}

