using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Common.Extensions;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Commands.Update;

public record ChangePasswordCommand(string OldPassword, string NewPassword) : IRequest<Adoroid.Core.Application.Wrappers.Response<bool>>;

public class ChangePasswordCommandHandler(ICurrentUser currentUser, IUnitOfWork unitOfWork, IAesEncryptionHelper aesEncryptionHelper) : IRequestHandler<ChangePasswordCommand, Adoroid.Core.Application.Wrappers.Response<bool>>
{
    public async Task<Adoroid.Core.Application.Wrappers.Response<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.ValidUserId();

        var user = await unitOfWork.Users.GetUserById(userId, cancellationToken);
        if (user == null)
        {
            return Adoroid.Core.Application.Wrappers.Response<bool>.Fail(BusinessExceptionMessages.UserNotFound);
        }

        var encyriptedOldPassword = aesEncryptionHelper.Encrypt(request.OldPassword);

        if(user.Password != encyriptedOldPassword)
        {
            return Adoroid.Core.Application.Wrappers.Response<bool>.Fail(BusinessExceptionMessages.OldPasswordIsIncorrect);
        }

        var newPassordEncyripted = aesEncryptionHelper.Encrypt(request.NewPassword);
        user.Password = newPassordEncyripted;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Adoroid.Core.Application.Wrappers.Response<bool>.Success(true);
    }
}