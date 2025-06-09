namespace Adoroid.CarService.Application.Features.Users.ExceptionMessages;

public class ValidationMessages
{
    public const string Required = "{0} alanı zorunludur.";
    public const string Email = "{0} alanı geçerli bir e-posta adresi olmalıdır.";
    public const string MinLength = "{0} alanı en az {1} karakter uzunluğunda olmalıdır.";
    public const string MaxLength = "{0} alanı en fazla {1} karakter uzunluğunda olmalıdır.";
    public const string NotEmpty = "{0} alanı boş bırakılamaz.";
    public const string NotNull = "{0} alanı null olamaz.";
}
