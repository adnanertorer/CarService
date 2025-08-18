namespace Adoroid.CarService.Application.Features.Reports.Dtos;

public class EmployeeServiceCountDto
{
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int ServiceCount { get; set; } = 0;
}
