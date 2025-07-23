using Adoroid.CarService.Domain.Entities;

namespace Adoroid.CarService.Application.Tests.Data;

public static class UserData
{
    public static readonly User User = new()
    {
        CreatedBy = Guid.NewGuid(),
        CreatedDate = DateTime.UtcNow,
        Email = "test@test.com",
        IsDeleted = false,
        Id = Guid.Parse("15ebf219-407a-498e-8637-f00e465fa3c9"),
        Name = "Test",
        Password = "Test1234!",
        RefreshToken = "test-refresh-token",
        PhoneNumber = "+90 555 123 4567",
        RefreshTokenExpr = DateTime.UtcNow.AddDays(7),
        Surname = "TestSurname"
    };
}
