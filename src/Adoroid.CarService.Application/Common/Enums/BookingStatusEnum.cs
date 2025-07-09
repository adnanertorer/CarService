using System.ComponentModel;

namespace Adoroid.CarService.Application.Common.Enums;

public enum BookingStatusEnum
{
    [Description("Waiting for confirmation")]
    Waiting = 0,
    [Description("Confirmed")]
    Accept = 0,
    [Description("Rejected")]
    Reject = 0
}
