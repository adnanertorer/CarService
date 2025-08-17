using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Records;
using Adoroid.CarService.Application.Features.Users.ExceptionMessages;
using Adoroid.CarService.Application.Features.Users.Templates;
using MassTransit;
using Microsoft.Extensions.Logging;
using MinimalMediatR.Core;

namespace Adoroid.CarService.Application.Features.Users.Queries.ResetPassword;

public record ResetPasswordRequestQuery(string Email) : IRequest<Adoroid.Core.Application.Wrappers.Response<bool>>; 

public class ResetPasswordRequestQueryHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint, ILogger<ResetPasswordRequestQueryHandler> logger) : IRequestHandler<ResetPasswordRequestQuery, Adoroid.Core.Application.Wrappers.Response<bool>>
{
    public async Task<Adoroid.Core.Application.Wrappers.Response<bool>> Handle(ResetPasswordRequestQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetUserWithEmailAddress(request.Email, cancellationToken);

        if (user is null)
        {
            return Adoroid.Core.Application.Wrappers.Response<bool>.Fail(BusinessExceptionMessages.UserNotFound);
        }

        string otpCode = new Random().Next(100000, 999999).ToString();
        user.OtpCode = otpCode;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Parola sıfırlama talebi için OTP kodu oluşturuldu: {OtpCode} for User ID: {UserId}", otpCode, user.Id);

        var mailBody = MailTemplates.GetPasswordResetEmailTemplate;
        mailBody = mailBody.Replace("{{OTP_CODE}}", otpCode)
            .Replace("{{ACTION_URL}}", "https://app.fixybear.com/reset-password")
            .Replace("{{SUPPORT_EMAIL}}", "info@fixybear.com")
            .Replace("{{CURRENT_YEAR}}", DateTime.UtcNow.Year.ToString())
            .Replace("{{PRIVACY_URL}}", "https://fixybear.com/privacy");

        var emailText = new SendMailRequest(new Common.Dtos.MailModel
        {
            Body = mailBody,
            IsBodyHtml = true,
            Recipient = user.Email,
            SenderDisplayName = "FixyBear",
            Subject = "FixyBear Parola Sıfırlama"
        });

        await publishEndpoint.Publish(emailText, cancellationToken);

        return Adoroid.Core.Application.Wrappers.Response<bool>.Success(true, "Parola sıfırlama talebi başarıyla gönderildi.");
    }
}
