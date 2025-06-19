using System.ComponentModel;

namespace Adoroid.CarService.Application.Common.Enums;

public enum TransactionTypeEnum
{
    [Description("Payable")]
    Payable = 0,
    [Description("Receivable")]
    Receivable = 1
}
