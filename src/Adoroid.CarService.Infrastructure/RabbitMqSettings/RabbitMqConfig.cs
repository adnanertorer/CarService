namespace Adoroid.CarService.Infrastructure.RabbitMqSettings;

public class RabbitMqConfig
{
    public string Host { get; set; } = default!;
    public int Port { get; set; } = 5672; // Default RabbitMQ port
    public string VirtualHost { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
