using System.ComponentModel;

namespace Adoroid.CarService.Application.Common.Enums;

public enum BookingStatusEnum
{
    [Description("Pending")]
    Pending = 0, 
    [Description("Approved")]
    Approved = 1,
    [Description("Rejected")]
    Rejected = 2,
    [Description("Cancelled")]
    Cancelled = 3, 
}
