using Adoroid.CarService.Application.Common.Abstractions.Auth;
using System.Security.Cryptography;
using System.Text;

namespace Adoroid.CarService.Infrastructure.Auth;

public class AesEncryptionHelper : IAesEncryptionHelper
{
    private static readonly string key = "1234567890123456"; 
    private static readonly string iv = "abcdefghijklmnop";  

    public string Encrypt(string plainText)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        aesAlg.IV = Encoding.UTF8.GetBytes(iv);

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        byte[] encrypted;
        using (MemoryStream ms = new())
        {
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var sw = new StreamWriter(cs))
                sw.Write(plainText);
            encrypted = ms.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string cipherText)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        aesAlg.IV = Encoding.UTF8.GetBytes(iv);

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        byte[] bytes = Convert.FromBase64String(cipherText);
        using var ms = new MemoryStream(bytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
