using Adoroid.CarService.Application.Common.Abstractions;
using Adoroid.CarService.Application.Common.Records;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Adoroid.CarService.Application.Features.Users.Consumers;

public class MailSenderConsumer : IConsumer<SendMailRequest>
{
    private readonly IMailSender _mailSender;
    private readonly ILogger<MailSenderConsumer> _logger;
    public MailSenderConsumer(IMailSender mailSender, ILogger<MailSenderConsumer> logger)
    {
        _mailSender = mailSender;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<SendMailRequest> context)
    {
        var mailModel = context.Message.MailModel;
        try
        {
            var result = await _mailSender.SendMail(mailModel);
            if (!result)
            {
                _logger.LogError("Failed to send email to {Recipient} with subject {Subject}", mailModel.Recipient, mailModel.Subject);
                throw new Exception("Failed to send email.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Recipient} with subject {Subject}", mailModel.Recipient, mailModel.Subject);
            throw new Exception("Error sending email: " + ex.Message, ex);
        }
    }
}
