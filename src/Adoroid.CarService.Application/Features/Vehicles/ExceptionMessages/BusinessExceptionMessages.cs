namespace Adoroid.CarService.Application.Features.Vehicles.ExceptionMessages;

public class BusinessExceptionMessages
{
    public const string NotFound = "Araç bulunamadı.";
    public const string AlreadyExists = "Bu araç zaten var.";
    public const string VehicleHasMainServices = "Bu araç üzerinde servis kayıtları olduğu için sizin tarafınızdan silinemez.Servis kayıt işlemlerinin silinmesi için ustayı bilgilendirin.";
    public const string VehicleUserRecordNotFound = "Araç kullanıcı kaydı bulunamadı.";
    public const string CustomerNotFound = "Müşteri bulunamadı.";
    public const string MobileUserNotFound = "Mobil kullanıcı bulunamadı.";
    public const string VehicleIsNotTemporary = "Araç bir müşteri üzerine atanmış, bu yüzden silinemez ya da değiştirilemez.";
}
