using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Queries.Logout;

public record UserLogoutQuery : IRequest<Adoroid.Core.Application.Wrappers.Response<bool>>;

public class UserLogoutQueryHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork) : IRequestHandler<UserLogoutQuery, Adoroid.Core.Application.Wrappers.Response<bool>>
{
    public async Task<Adoroid.Core.Application.Wrappers.Response<bool>> Handle(UserLogoutQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetUserById(Guid.Parse(currentUser.Id!), cancellationToken);

        if(user == null)
            return Adoroid.Core.Application.Wrappers.Response<bool>.Fail(BusinessExceptionMessages.UserNotFound);

        user.RefreshToken = null;
        user.RefreshTokenExpr = null;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new Core.Application.Wrappers.Response<bool> { Data = true, Succeeded = true, Message = "User logged out successfully."  };
    }
}
