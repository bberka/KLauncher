using System.Security.Cryptography;
using System.Text;
using System.Data.HashFunction;
using System.Data.HashFunction.xxHash;
using System.Security.Cryptography;

namespace KLauncher.Core.Manager;

public static class HashManager
{


#region MD5

    public static byte[] Md5(string input) {
        using var hash = MD5.Create();
        return hash.ComputeHash(Encoding.UTF8.GetBytes(input));
    }

    public static string Md5AsHexString(string input) {
        using var hash = MD5.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder();
        foreach (var t in bytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

    public static string Md5AsBase64String(string input) {
        using var hash = MD5.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
    
    public static string Md5HashFileByFilePath(string path) {
        var bytes = File.ReadAllBytes(path);
        using var hash = MD5.Create();
        var hashBytes = hash.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }
    
    public static string Md5HashFileByFileBytes(byte[] bytes) {
        using var hash = MD5.Create();
        var hashBytes = hash.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }
    public static string Md5HashFileByStream(Stream stream) {
        using var hash = MD5.Create();
        var hashBytes = hash.ComputeHash(stream);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }
#endregion


#region SHA256

 
    public static byte[] Sha256(string input) {
        using var hash = SHA256.Create();
        return hash.ComputeHash(Encoding.UTF8.GetBytes(input));
    }

    public static string Sha256AsHexString(string input) {
        using var hash = SHA256.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder();
        foreach (var t in bytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

    public static string Sha256AsBase64String(string input) {
        using var hash = SHA256.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
    
    public static string Sha256HashFileByFilePath(string path) {
        var bytes = File.ReadAllBytes(path);
        using var hash = SHA256.Create();
        var hashBytes = hash.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }
    public static string Sha256HashFileByFileBytes(byte[] bytes) {
        using var hash = SHA256.Create();
        var hashBytes = hash.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }
    public static string Sha256HashFileByStream(Stream stream) {
        using var hash = SHA256.Create();
        var hashBytes = hash.ComputeHash(stream);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }
#endregion
    
    //public string Md5HashFile(string path) {
    //    var bytes = File.ReadAllBytes(path);
    //    using var hash = MD5.Create();
    //    var hashBytes = hash.ComputeHash(bytes);
    //    var sb = new StringBuilder();
    //    foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
    //    return sb.ToString();
    //}
    
   
#region XXHASH
    public static string XxHashFileByFilePath(string path) {
        var factory = xxHashFactory.Instance.Create();
        using var stream = File.OpenRead(path);
        var hashed = factory.ComputeHash(stream).AsHexString();
        return hashed;
    }

    public static string XxHashFileByFileBytes(byte[] bytes) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(bytes).AsHexString();
        return hashed;
    }

    public static string XxHashFileByStream(Stream stream) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(stream).AsHexString();
        return hashed;
    }

    public static string XxHashAsHexString(string text) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(text).AsHexString();
        return hashed;
    }
    public static string XxHashAsBase64String(string text) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(text).AsBase64String();
        return hashed;
    }
    public static byte[] XxHash(string text) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(text);
        return hashed.Hash;
    }
    

#endregion
   
}