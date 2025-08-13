using Adoroid.CarService.Application.Common.Dtos;

namespace Adoroid.CarService.Application.Common.Abstractions;

public interface IMailSender
{
    Task<bool> SendMail(MailModel mailModel);
}
