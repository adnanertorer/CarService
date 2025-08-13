using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Dtos;
using Adoroid.Core.Application.Exceptions.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Adoroid.CarService.Infrastructure.Mail;

public class MailSender(IOptions<MailConfig> options, ILogger<MailSender> logger) : IMailSender
{
    private readonly MailConfig _mailConfig = options.Value;
    public async Task<bool> SendMail(MailModel mailModel)
    {
        try
        {
            SmtpClient smtpClient = new()
            {
                UseDefaultCredentials = false,
                EnableSsl = _mailConfig.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_mailConfig.MailUsername, _mailConfig.MailPassword),
                Host = _mailConfig.SmtpHost,
                Port = _mailConfig.SmtpPort
            };

            MailAddress from = new(_mailConfig.MailSender, mailModel.SenderDisplayName);
            MailAddress to = new(mailModel.Recipient);
            MailMessage mail = new(from, to)
            {
                Subject = mailModel.Subject,
                SubjectEncoding = System.Text.Encoding.UTF8,

                Body = mailModel.Body,
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = mailModel.IsBodyHtml
            };

            await smtpClient.SendMailAsync(mail);
            return true;
        }

        catch (SmtpException ex)
        {
            logger.LogError(ex, "SMTP Exception occurred while sending mail. Subject: {Subject}, Recipient: {Recipient}", mailModel.Subject, mailModel.Recipient);
            throw new CustomException(HttpStatusCode.InternalServerError, "Mail Sender Error", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while sending mail. Subject: {Subject}, Recipient: {Recipient}", mailModel.Subject, mailModel.Recipient);
            throw new CustomException(HttpStatusCode.InternalServerError, "Error", ex.Message);
        }
    }
}
