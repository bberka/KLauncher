using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace KLauncher.Core.Manager;

/// <summary>
///     Encryption manager for encrypting and decrypting strings. Different instances of this class will have different
///     keys.
///     For client and server matching key, key is generated based on a time formula. It will be same if created in same
///     second.
/// </summary>
public class EncryptionManager
{
    private const string _additionalKey = "XmMqPLuQkkrKaNmCvRGyceZNDdquhTJokckfPdcKPjeekkooeaSmBGDNwEaqDFJq";
    private readonly byte[] ivBytes;
    private readonly byte[] keyBytes;

    public EncryptionManager() {
        var key = CreateKey();
        using var sha256 = SHA256.Create();
        var keyHash = sha256.ComputeHash(key);
        keyBytes = new byte[32];
        ivBytes = new byte[16];
        Array.Copy(keyHash, keyBytes, 32);
        Array.Copy(keyHash, 0, ivBytes, 0, 16);
    }

    private static DateTime GetDate() {
        var now = DateTime.UtcNow;
        var second = now.Second;
        var ms = now.Millisecond;
        now = now.AddHours(+second + ConstManager.BuildNumber);
        now = now.AddYears(-second - ConstManager.BuildNumber);
        now = now.AddMonths(+second + ConstManager.BuildNumber);
        now = now.AddDays(-second - ConstManager.BuildNumber);
        now = now.AddMinutes(+second + ConstManager.BuildNumber);
        now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        return now;
    }

    private static byte[] CreateKey() {
        var time = GetDate();
        var ticks = time.Ticks;
        var str = time.ToString("MM-dd-yyyy-HH-mm-ss") + "|" + _additionalKey;
        var key = HashManager.Hash(str);
        return key;
    }

    public string Encrypt(string plainText) {
        if (ConstManager.IsDevelopment) return plainText;
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = ivBytes;
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(encryptedBytes);
    }

    public string Decrypt(string encryptedText) {
        if (ConstManager.IsDevelopment) return encryptedText;
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = ivBytes;
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}