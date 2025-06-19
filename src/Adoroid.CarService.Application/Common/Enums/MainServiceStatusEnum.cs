using System.ComponentModel;

namespace Adoroid.CarService.Application.Common.Enums;

public enum MainServiceStatusEnum
{
    [Description("Opened")]
    Opened = 0,
    [Description("Preparing")]
    Prepared = 1,
    [Description("Done")]
    Done = 2,
    [Description("Cancelled")]
    Cancelled = 3
}
