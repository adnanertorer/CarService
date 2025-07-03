using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.BusinessMessages;
using Adoroid.Core.Application.Wrappers;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Queries.CheckCompanyId;

public record GetCompanyIdCommand() : IRequest<Response<Guid?>>;

public class GetCompanyIdCommandHandler(ICurrentUser currentUser) : IRequestHandler<GetCompanyIdCommand, Response<Guid?>>
{
    public Task<Response<Guid?>> Handle(GetCompanyIdCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(currentUser.CompanyId))
        {
            if (!Guid.TryParse(currentUser.CompanyId, out _))
                return Task.FromResult(Response<Guid?>.Fail(BusinessMessages.InvalidCompanyId));
        }
        else
        {
            return Task.FromResult(Response<Guid?>.Fail(BusinessMessages.CompanyNotFound));
        }

        return Task.FromResult(Response<Guid?>.Success(Guid.Parse(currentUser.CompanyId!)));
    }
}
