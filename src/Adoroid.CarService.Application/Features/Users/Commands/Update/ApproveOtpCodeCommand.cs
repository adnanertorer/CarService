using Adoroid.CarService.Application.Common.Abstractions;
using MinimalMediatR.Core;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;

namespace Adoroid.CarService.Application.Features.Users.Commands.Update;

public record ApproveOtpCodeCommand(string OtpCode) : IRequest<Core.Application.Wrappers.Response<bool>>;

public class ApproveOtpCodeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ApproveOtpCodeCommand, Core.Application.Wrappers.Response<bool>>
{
    public async Task<Core.Application.Wrappers.Response<bool>> Handle(ApproveOtpCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetUserByOtpCodeAsync(request.OtpCode, cancellationToken);
        
        if (user is null)
            return Core.Application.Wrappers.Response<bool>.Fail(BusinessExceptionMessages.OtpInvalid);

        user.IsActive = true;
        user.OtpCode = null; 

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Core.Application.Wrappers.Response<bool>.Success(true);
    }
}
