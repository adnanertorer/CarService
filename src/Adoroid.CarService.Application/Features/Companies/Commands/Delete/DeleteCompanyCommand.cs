using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Companies.ExceptionMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Companies.Commands.Delete;

public record DeleteCompanyCommand(Guid Id) : IRequest<Response<Guid>>;

public class DeleteCompanyCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) :
    IRequestHandler<DeleteCompanyCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await unitOfWork.Companies.GetByIdAsync(request.Id, cancellationToken);
        if (company == null)
            return Response<Guid>.Fail(BusinessExceptionMessages.CompanyNotFound);

        company.IsDeleted = true;
        company.DeletedDate = DateTime.Now;
        company.DeletedBy = Guid.Parse(currentUser.Id!);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Response<Guid>.Success(company.Id);
    }
}
