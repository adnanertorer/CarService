namespace Adoroid.CarService.Application.Features.UserToCompanies.Dtos;

public class UserToCompanyDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public int UserType { get; set; }
}
