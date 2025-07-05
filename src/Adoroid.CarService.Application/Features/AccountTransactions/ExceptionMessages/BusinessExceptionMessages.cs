namespace Adoroid.CarService.Application.Features.AccountTransactions.ExceptionMessages;

public static class BusinessExceptionMessages
{
    public const string NotFound = "Cari hesap hareketi bulunamadı.";
    public const string MainServiceIdCannotBeNull = "Ana hizmet ID'si boş olamaz. Bu işlem sadece hizmet işlem kayıtları için yapılabilir.";
    public const string MainServiceNotFound = "Ana hizmet bulunamadı.";
    public const string CustomerNotFound = "Müşteri bulunamadı.";
}
