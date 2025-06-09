namespace Adoroid.CarService.Application.Features.Companies.ExceptionMessages;

public static class BusinessExceptionMessages
{
    public const string CompanyNotFound = "Şirket bulunamadı.";
    public const string CompanyAlreadyExists = "Bu vergi numarası veya e-posta ile kayıtlı bir şirket zaten var.";
    public const string InvalidCompanyData = "Geçersiz şirket verileri.";
    public const string UnauthorizedCompanyAccess = "Şirkete yetkisiz erişim.";
    public const string CompanyCreationFailed = "Şirket oluşturma işlemi başarısız oldu.";
    public const string CompanyUpdateFailed = "Şirket güncelleme işlemi başarısız oldu.";
    public const string CompanyDeletionFailed = "Şirket silme işlemi başarısız oldu.";
}
