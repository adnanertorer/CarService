using System.ComponentModel;

namespace Adoroid.CarService.Application.Common.Enums;

public enum VehicleUserTypeEnum
{
    [Description("Master User")]
    Master = 1,
    [Description("Driver User")]
    Driver = 2,
    [Description("Temporary User")]
    Temporary = 3
}
