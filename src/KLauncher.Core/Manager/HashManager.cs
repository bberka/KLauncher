using System.Security.Cryptography;
using System.Text;
using System.Data.HashFunction;
using System.Data.HashFunction.xxHash;
using System.Security.Cryptography;
using KLauncher.Shared;
using KLauncher.Shared.Enum;

namespace KLauncher.Core.Manager;

public static class HashManager
{
    public static string HashAsHexString(string input) {
        return SharedConfig.HashType switch {
            HashType.Md5 => Md5AsHexString(input),
            HashType.Sha256 => Sha256AsHexString(input),
            HashType.XxHash => XxHashAsHexString(input),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    public static string HashFileByFilePath(string path) {
        return SharedConfig.HashType switch {
            HashType.Md5 => Md5HashFileByFilePath(path),
            HashType.Sha256 => Sha256HashFileByFilePath(path),
            HashType.XxHash => XxHashFileByFilePath(path),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string HashFileByFileBytes(byte[] bytes) {
        return SharedConfig.HashType switch {
            HashType.Md5 => Md5HashFileByFileBytes(bytes),
            HashType.Sha256 => Sha256HashFileByFileBytes(bytes),
            HashType.XxHash => XxHashFileByFileBytes(bytes),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string HashFileByStream(Stream stream) {
        return SharedConfig.HashType switch {
            HashType.Md5 => Md5HashFileByStream(stream),
            HashType.Sha256 => Sha256HashFileByStream(stream),
            HashType.XxHash => XxHashFileByStream(stream),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string HashString(string text) {
        return SharedConfig.HashType switch {
            HashType.Md5 => Md5AsHexString(text),
            HashType.Sha256 => Sha256AsHexString(text),
            HashType.XxHash => XxHashAsHexString(text),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string HashStringAsBase64(string text) {
        return SharedConfig.HashType switch {
            HashType.Md5 => Md5AsBase64String(text),
            HashType.Sha256 => Sha256AsBase64String(text),
            HashType.XxHash => XxHashAsBase64String(text),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static byte[] Hash(string text) {
        return SharedConfig.HashType switch {
            HashType.Md5 => Md5(text),
            HashType.Sha256 => Sha256(text),
            HashType.XxHash => XxHash(text),
            _ => throw new ArgumentOutOfRangeException()
        };
    }


#region MD5

    private static byte[] Md5(string input) {
        using var hash = MD5.Create();
        return hash.ComputeHash(Encoding.UTF8.GetBytes(input));
    }

    private static string Md5AsHexString(string input) {
        using var hash = MD5.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder();
        foreach (var t in bytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

    private static string Md5AsBase64String(string input) {
        using var hash = MD5.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }

    private static string Md5HashFileByFilePath(string path) {
        var bytes = File.ReadAllBytes(path);
        using var hash = MD5.Create();
        var hashBytes = hash.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

    private static string Md5HashFileByFileBytes(byte[] bytes) {
        using var hash = MD5.Create();
        var hashBytes = hash.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

    private static string Md5HashFileByStream(Stream stream) {
        using var hash = MD5.Create();
        var hashBytes = hash.ComputeHash(stream);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

#endregion


#region SHA256

    private static byte[] Sha256(string input) {
        using var hash = SHA256.Create();
        return hash.ComputeHash(Encoding.UTF8.GetBytes(input));
    }

    private static string Sha256AsHexString(string input) {
        using var hash = SHA256.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder();
        foreach (var t in bytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

    private static string Sha256AsBase64String(string input) {
        using var hash = SHA256.Create();
        var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }

    private static string Sha256HashFileByFilePath(string path) {
        var bytes = File.ReadAllBytes(path);
        using var hash = SHA256.Create();
        var hashBytes = hash.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

    private static string Sha256HashFileByFileBytes(byte[] bytes) {
        using var hash = SHA256.Create();
        var hashBytes = hash.ComputeHash(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes) sb.Append(t.ToString("x2"));
        return sb.ToString();
    }

    private static string Sha256HashFileByStream(Stream stream) {
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

    private static string XxHashFileByFilePath(string path) {
        var factory = xxHashFactory.Instance.Create();
        using var stream = File.OpenRead(path);
        var hashed = factory.ComputeHash(stream).AsHexString();
        return hashed;
    }

    private static string XxHashFileByFileBytes(byte[] bytes) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(bytes).AsHexString();
        return hashed;
    }

    private static string XxHashFileByStream(Stream stream) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(stream).AsHexString();
        return hashed;
    }

    private static string XxHashAsHexString(string text) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(text).AsHexString();
        return hashed;
    }

    private static string XxHashAsBase64String(string text) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(text).AsBase64String();
        return hashed;
    }

    private static byte[] XxHash(string text) {
        var factory = xxHashFactory.Instance.Create();
        var hashed = factory.ComputeHash(text);
        return hashed.Hash;
    }

#endregion
}