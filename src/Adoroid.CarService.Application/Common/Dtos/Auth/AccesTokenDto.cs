namespace Adoroid.CarService.Application.Common.Dtos.Auth;

public class AccesTokenDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; }
    public string FullName { get; set; }
    public Guid UserId { get; set; }
    public Guid? CompanyId { get; set; }
}
