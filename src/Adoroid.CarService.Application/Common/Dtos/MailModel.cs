namespace Adoroid.CarService.Application.Common.Dtos;

public class MailModel
{
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string SenderDisplayName { get; set; } = default!;
    public string Recipient { get; set; } = default!;
    public bool IsBodyHtml { get; set; }
}
