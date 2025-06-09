namespace Adoroid.CarService.Application.Features.Users.ExceptionMessages;

public static class BusinessExceptionMessages
{
    public const string UserNotFound = "Kullanıcı bulunamadı.";
    public const string InvalidCredentials = "Geçersiz kimlik bilgileri.";
    public const string UserAlreadyExists = "Bu e-posta adresiyle kayıtlı bir kullanıcı zaten var.";
    public const string PasswordMismatch = "Parolalar eşleşmiyor.";
    public const string UnauthorizedAccess = "Yetkisiz erişim.";
    public const string AccountLocked = "Hesabınız kilitlenmiştir. Lütfen destek ile iletişime geçin.";
    public const string LoginFailed = "Giriş başarısız oldu. Lütfen tekrar deneyin.";
    public const string InvalidRefreshToken = "Geçersiz yenileme jetonu.";
    public const string RefreshTokenExpired = "Yenileme jetonu süresi dolmuş.";
}
