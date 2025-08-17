using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Commands.Update;

public record ApproveResetPasswordCommand(string OtpCode, string NewPassword) : IRequest<Adoroid.Core.Application.Wrappers.Response<bool>>;

public class ApproveResetPasswordCommandHandler(IUnitOfWork unitOfWork, IAesEncryptionHelper aesEncryptionHelper, ILogger<ApproveResetPasswordCommandHandler>
     logger) : IRequestHandler<ApproveResetPasswordCommand, Adoroid.Core.Application.Wrappers.Response<bool>>
{
    public async Task<Adoroid.Core.Application.Wrappers.Response<bool>> Handle(ApproveResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetUserByOtpCodeAsync(request.OtpCode, cancellationToken);

        if (user is null)
        {
            return Adoroid.Core.Application.Wrappers.Response<bool>.Fail(BusinessExceptionMessages.OtpInvalid);
        }

        var encryptedPassword = aesEncryptionHelper.Encrypt(request.NewPassword);

        if (encryptedPassword is null)
        {
            logger.LogError("Parola şifreleme başarısız oldu. OtpCode: {OtpCode}", request.OtpCode);
            return Adoroid.Core.Application.Wrappers.Response<bool>.Fail(BusinessExceptionMessages.UnexpectedError);
        }

        user.Password = encryptedPassword; 
        user.OtpCode = null; 
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Adoroid.Core.Application.Wrappers.Response<bool>.Success(true, "Parola başarıyla sıfırlandı.");
    }
}
