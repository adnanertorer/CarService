namespace Adoroid.CarService.Application.Features.MainServices.ExceptionMessages;

public class BusinessExceptionMessages
{
    public const string NotFound = "Servis bulunamadı.";
    public const string AlreadyExists = "Bu servis zaten var.";
    public const string VehicleNotFound = "Araç sistemde kayıtlı değil";
    public const string VehicleUserNotFound = "Araç sahibi sistemde kayıtlı değil";
    public const string MainServiceUpdateError = "Servis güncellenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
    public const string NewServiceDateBeforeOld = "Yeni servis tarihi eski tarihten önce olamaz.";
}
