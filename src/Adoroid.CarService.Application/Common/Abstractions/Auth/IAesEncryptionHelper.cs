namespace Adoroid.CarService.Application.Common.Abstractions.Auth;

public interface IAesEncryptionHelper
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}
