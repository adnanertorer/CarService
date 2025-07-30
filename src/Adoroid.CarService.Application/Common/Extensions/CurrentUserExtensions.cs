using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Adoroid.Core.Application.Exceptions.Types;

namespace Adoroid.CarService.Application.Common.Extensions;

public static class CurrentUserExtensions
{
    public static Guid ValidCompanyId(this ICurrentUser currentUser)
    {
        if (string.IsNullOrWhiteSpace(currentUser.CompanyId))
            throw new BusinessException(BusinessMessages.BusinessMessages.CompanyNotFound);

        if (!Guid.TryParse(currentUser.CompanyId, out var companyId))
            throw new BusinessException(BusinessMessages.BusinessMessages.InvalidCompanyId);

        return companyId;
    }

    public static Guid ValidUserId(this ICurrentUser currentUser)
    {
        if (string.IsNullOrWhiteSpace(currentUser.Id))
            throw new BusinessException(BusinessMessages.BusinessMessages.CompanyNotFound);

        if (!Guid.TryParse(currentUser.Id, out var userId))
            throw new BusinessException(BusinessMessages.BusinessMessages.InvalidCompanyId);

        return userId;
    }
}
