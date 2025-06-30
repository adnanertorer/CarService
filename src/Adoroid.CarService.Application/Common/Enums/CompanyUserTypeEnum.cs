using System.ComponentModel;

namespace Adoroid.CarService.Application.Common.Enums;

public enum CompanyUserTypeEnum
{
    [Description("Master User")]
    Master = 1,
    [Description("Employee User")]
    Employee = 2
}
