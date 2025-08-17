namespace Adoroid.CarService.Application.Features.Users.ExceptionMessages;

public static class BusinessExceptionMessages
{
    public const string UserNotFound = "Kullanıcı bulunamadı.";
    public const string InvalidCredentials = "Geçersiz kimlik bilgileri.";
    public const string UserAlreadyExists = "Bu e-posta adresi ya da telefon numarasıyla kayıtlı bir kullanıcı zaten var.";
    public const string PasswordMismatch = "Parolalar eşleşmiyor.";
    public const string UnauthorizedAccess = "Yetkisiz erişim.";
    public const string AccountLocked = "Hesabınız kilitlenmiştir. Lütfen destek ile iletişime geçin.";
    public const string LoginFailed = "Giriş başarısız oldu. Lütfen tekrar deneyin.";
    public const string InvalidRefreshToken = "Geçersiz yenileme jetonu.";
    public const string RefreshTokenExpired = "Yenileme jetonu süresi dolmuş.";
    public const string OtpRequired = "OTP doğrulaması gereklidir.";
    public const string OtpInvalid = "Geçersiz OTP kodu.";
    public const string OtpExpired = "OTP kodu süresi dolmuş.";
    public const string OtpAlreadySent = "OTP kodu zaten gönderildi. Lütfen bekleyin ve tekrar deneyin.";
    public const string OtpNotSent = "OTP kodu gönderilemedi. Lütfen tekrar deneyin.";
    public const string OtpLimitExceeded = "OTP gönderim limiti aşıldı. Lütfen daha sonra tekrar deneyin.";
    public const string CompanyNotFound = "Şirket bulunamadı.";
    public const string CompanyAlreadyExists = "Bu vergi numarasıyla kayıtlı bir şirket zaten var.";
    public const string AlreadyExistsCompanyUser = "Bu kullanıcı zaten bu şirkette kayıtlı.";
    public const string UnexpectedError = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
}
