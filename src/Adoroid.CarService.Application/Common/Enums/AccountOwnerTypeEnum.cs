using System.ComponentModel;

namespace Adoroid.CarService.Application.Common.Enums;

public enum AccountOwnerTypeEnum
{
    [Description("Customer")]
    Customer = 1,
    [Description("Mobile User")]
    MobileUser = 2, 
    [Description("Company")]
    Company = 3
}
