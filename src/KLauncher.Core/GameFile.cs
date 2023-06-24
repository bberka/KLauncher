using KLauncher.Core.Manager;

namespace KLauncher.Core;

public class GameFile : GameFileWithoutHash, IEquatable<GameFile>
{
    internal const long MaxFileSize = 1024 * 1024 * 1024; //1GB
    internal const bool CheckZeroByteFiles = true;

    internal static readonly string[] IgnoreFileExtensions = {
        ".log"
    };

    public GameFile(string pathFromRoot, string hash, long size, long lastUpdate) {
        RelativePath = pathFromRoot;
        Hash = hash;
        Size = size;
        LastUpdate = lastUpdate;
    }

    public GameFile() {
    }

    // /// <summary>
    // /// File relative path from root directory.
    // /// </summary>
    // public string RelativePath { get; init; }
    //
    // /// <summary>
    // /// Relative path hash string to compare files in same location and name. All / and \ are replaced with * to avoid path issues.
    // /// This value is used to make sure the file path is the same not the actual content.
    // /// </summary>
    // public string PathHash => HashManager.XxHashAsHexString(
    //     RelativePath 
    //     .Replace("\\","*")
    //     .Replace("/","*"));

    /// <summary>
    ///     File bytes hash string to compare files content.
    /// </summary>
    public string Hash { get; init; }

    public bool Equals(GameFile? other) {
        if (ReferenceEquals(null, other)) return false;
        return other.Hash == Hash;
    }

    public static ResultData<GameFile> Read(string rootPath, string fullFilePath) {
        if (!File.Exists(fullFilePath)) return Result.Error("FileDoesNotExists");
        var fileInfo = new FileInfo(fullFilePath);
        var fileSize = fileInfo.Length;
        if (fileSize > MaxFileSize) return Result.Error("FileTooBig", GetByteAsString(fileSize));
        if (CheckZeroByteFiles)
            if (fileSize == 0)
                return Result.Error("FileIsEmpty");
        var stream = fileInfo.OpenRead();
        var hash = HashManager.HashFileByStream(stream);
        stream.Close();
        stream.Flush();
        var pathFromRoot = fullFilePath.Replace(rootPath, "");

        return new GameFile(pathFromRoot, hash, fileInfo.Length, fileInfo.LastWriteTimeUtc.Ticks);
    }
    // /// <summary>
    // /// File size in bytes. Max file size is 1GB.
    // /// </summary>
    // public long Size { get; init; }
    //
    // /// <summary>
    // /// Last file update time in ticks (UTC).
    // /// </summary>
    // public long LastUpdate { get; init; }

    public static bool operator ==(GameFile gameFile, GameFile gameFile2) {
        return Equals(gameFile, gameFile2);
    }

    public static bool operator !=(GameFile gameFile, GameFile gameFile2) {
        return !Equals(gameFile, gameFile2);
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != GetType()) return false;
        return ((GameFile)obj).Hash == Hash;
    }

    public override int GetHashCode() {
        return HashCode.Combine(LastUpdate, RelativePath, Hash, Size);
    }

    private static string GetByteAsString(long bytes) {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024} KB";
        if (bytes < 1024 * 1024 * 1024) return $"{bytes / 1024 / 1024} MB";
        return $"{bytes / 1024 / 1024 / 1024} GB";
    }

    public GameFileWithoutHash WithoutHash() {
        return new GameFileWithoutHash {
            Size = Size,
            LastUpdate = LastUpdate,
            RelativePath = RelativePath
        };
    }
}