namespace Adoroid.CarService.Application.Common.Abstractions.Auth;

public interface ICurrentUser
{
    string? Id { get; }
    string? CompanyId { get; }
    string? UserName { get; }
    string? Email { get; }
    string? Token { get; }
    string? UserType { get; }
    List<string>? Roles { get; }
}
