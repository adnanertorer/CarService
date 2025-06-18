namespace Adoroid.CarService.Application.Common.ValidationMessages;

public static class ValidationMessages
{
    public const string Required = "{0} alanı zorunludur.";
    public const string Email = "{0} alanı geçerli bir e-posta adresi olmalıdır.";
    public const string MinLength = "{0} alanı en az {1} karakter uzunluğunda olmalıdır.";
    public const string MaxLength = "{0} alanı en fazla {1} karakter uzunluğunda olmalıdır.";
    public const string NotEmpty = "{0} alanı boş bırakılamaz.";
    public const string NotNull = "{0} alanı null olamaz.";
    public const string GreaterThanZero = "{0} alanı sıfırdan büyük olmak zorunda";
    public const string PageRequestRequired = "Page request required";
    public const string PageRequestPageSizeMustBeGreaterThanZero = "Page size must be greater than 0.";
    public const string PageRequestPageSizeMustBeLessThan100 = "Page size must be less than 100.";
    public const string GreaterThanNow = "{0} şu anki zamandan küçük olamaz";
    public const string StartDateMustBeLessThanEndDate = "Başlangıç tarihi bitiş tarihinden küçük olmalıdır";
}
