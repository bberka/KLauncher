namespace KLauncher.Shared.Manager;

/// <summary>
/// Singleton EncryptManager class. Provides methods for encrypting and decrypting data.
/// </summary>
public class EncryptManager
{

    private EncryptManager() { }
	public static EncryptManager This {
		get {
			Instance ??= new();
			return Instance;
		}
	}
	private static EncryptManager? Instance;

    //TODO: Must SET encryption key
	private readonly string _encryptionKey = "";

    public string Encrypt(string input) {
		//encrypt string input with encryption key

		if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
        using var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = System.Text.Encoding.ASCII.GetBytes(_encryptionKey);
        aes.IV = System.Text.Encoding.ASCII.GetBytes(_encryptionKey);
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new System.IO.MemoryStream();
        using var cs = new System.Security.Cryptography.CryptoStream(ms, encryptor, System.Security.Cryptography.CryptoStreamMode.Write);
        using var sw = new System.IO.StreamWriter(cs);
        sw.Write(input);
        return System.Convert.ToBase64String(ms.ToArray());

    }

    public string Decrypt(string encryptedText) {
        //decrypt string encryptedText with encryption key
        if (string.IsNullOrEmpty(encryptedText)) throw new ArgumentNullException(nameof(encryptedText));
        using var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = System.Text.Encoding.ASCII.GetBytes(_encryptionKey);
        aes.IV = System.Text.Encoding.ASCII.GetBytes(_encryptionKey);
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new System.IO.MemoryStream(System.Convert.FromBase64String(encryptedText));
        using var cs = new System.Security.Cryptography.CryptoStream(ms, decryptor, System.Security.Cryptography.CryptoStreamMode.Read);
        using var sr = new System.IO.StreamReader(cs);
        return sr.ReadToEnd();
    }

}