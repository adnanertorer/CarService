namespace Adoroid.CarService.Shared;

public interface ICurrentUser
{
    string? Id { get; }
    string? CompanyId { get; }
    string? UserName { get; }
    string? Email { get; }
    string? Token { get; }
    List<string>? Roles { get; }
}
