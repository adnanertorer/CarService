using Adoroid.CarService.Application.Common.Abstractions.Auth;
using Moq;

namespace Adoroid.CarService.Application.Tests.Helpers;

public static class CurrentUserMockHelper
{
    public static Mock<ICurrentUser> Create(string userId, string companyId)
    {
        var mock = new Mock<ICurrentUser>();
        mock.Setup(x => x.Id).Returns(userId);
        mock.Setup(x => x.CompanyId).Returns(companyId);
        mock.Setup(x => x.UserName).Returns("adnanertorer");
        mock.Setup(x => x.Email).Returns("adnan@example.com");
        mock.Setup(x => x.Token).Returns("mocked_token");
        mock.Setup(x => x.UserType).Returns("company");
        mock.Setup(x => x.Roles).Returns(new List<string> { "Admin" });
        return mock;
    }
}

