namespace Adoroid.CarService.Infrastructure.Mail;

public class MailConfig
{
    public string MailPassword { get; set; } = default!;
    public string MailSender { get; set; } = default!;
    public string MailSenderDisplayName { get; set; } = default!;
    public string SmtpHost { get; set; } = default!;
    public int SmtpPort { get; set; }
    public string MailUsername { get; set; } = default!;
    public bool EnableSsl { get; set; }
}
