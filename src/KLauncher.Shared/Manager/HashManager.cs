using KLauncher.Shared.Interface;

namespace KLauncher.Shared.Manager;

/// <summary>
/// Singleton HashManager class for hashing strings with MD5, SHA1, SHA256, SHA384, SHA512
/// </summary>
public class HashManager
{
    private HashManager() { }
    public static HashManager This {
        get {
            Instance ??= new();
            return Instance;
        }
    }
    private static HashManager? Instance;
    public string MD5(string input) {
        if(string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);
        var sb = new System.Text.StringBuilder();
        foreach(var t in hashBytes) sb.Append(t.ToString("X2"));
        return sb.ToString();
    }

    public string SHA1(string input) {
        if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
        using var sha1 = System.Security.Cryptography.SHA1.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hashBytes = sha1.ComputeHash(inputBytes);
        var sb = new System.Text.StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("X2"));
        return sb.ToString();

    }

    public string SHA256(string input) {
        if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hashBytes = sha256.ComputeHash(inputBytes);
        var sb = new System.Text.StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("X2"));
        return sb.ToString();
    }

    public string SHA384(string input) {
        if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
        using var sha256 = System.Security.Cryptography.SHA384.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hashBytes = sha256.ComputeHash(inputBytes);
        var sb = new System.Text.StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("X2"));
        return sb.ToString();
    }

    public string SHA512(string input) {
        if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
        using var sha256 = System.Security.Cryptography.SHA512.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hashBytes = sha256.ComputeHash(inputBytes);
        var sb = new System.Text.StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("X2"));
        return sb.ToString();
    }
}