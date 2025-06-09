using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using Adoroid.CarService.Persistence;
using Adoroid.Core.Application.Wrappers;
using Microsoft.EntityFrameworkCore;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Commands.Delete;

public record DeleteCompanyCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteCompanyCommandHandler(CarServiceDbContext dbContext, ICurrentUser currentUser) :
    IRequestHandler<DeleteCompanyCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (company == null)
            return Response<Guid>.Fail(BusinessExceptionMessages.CompanyNotFound);

        company.IsDeleted = true;
        company.DeletedDate = DateTime.Now;
        company.DeletedBy = Guid.Parse(currentUser.Id!);

        dbContext.Update(company);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(company.Id);
    }
}
