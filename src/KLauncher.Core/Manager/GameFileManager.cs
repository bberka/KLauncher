using KLauncher.Core.Models;
using KLauncher.Shared.Models;
using Microsoft.Extensions.Options;
using Serilog;

namespace KLauncher.Core.Manager;

public class GameFileManager
{
    private const int ParallelismDegree = 5;
    private static readonly object GetFiles_FilesReadLock = new();

    private readonly string _rootPath;
    //     var files = new List<GameFileWithoutHash>();
    //     var rootDirectory = new DirectoryInfo(GetRootPath());
    //     var existingFileList = rootDirectory
    //         .GetFiles("*", SearchOption.AllDirectories)
    //         .Where(x => !IsFileIgnored(x.FullName))
    //         .ToArray();
    //     Parallel.ForEach(existingFileList, new ParallelOptions() {
    //         MaxDegreeOfParallelism = ParallelismDegree
    //     }, file => {
    //         var relativePath = file.FullName.Replace(GetRootPath(), "");
    //         var gameFile = new GameFileWithoutHash() {
    //             RelativePath = relativePath,
    //             Size = file.Length,
    //             LastUpdate = file.LastWriteTimeUtc.Ticks,
    //         };
    //         lock (files) {
    //             files.Add(gameFile);
    //         }
    //     });
    //     Log.Debug("{RemovedFileCount}:{NewFileCount}:{FileCount}", removedCount, newFileCount, files.Count);
    //     return files;
    // }
    //
    //

    private List<GameFile> _cachedFiles = new();

    private List<GameFileWithoutHash> _cachedFilesWithoutHash = new();

    public GameFileManager(IOptions<RootDirectory> directory) {
        _rootPath = directory.Value.Path;
    }
    // private static string _rootPath;

    private string GetRootPath() {
        if (string.IsNullOrEmpty(_rootPath)) throw new Exception("Root path is not set");
        return _rootPath;
    }

    public void SetRootPath(string rootPath) {
        // _rootPath = rootPath;
    }

    private static bool IsFileIgnored(string path) {
        return GameFile.IgnoreFileExtensions.Any(path.EndsWith);
    }

    public List<GameFileWithoutHash> GetFilesWithoutHash() {
        var files = new List<GameFileWithoutHash>();
        var rootDirectory = new DirectoryInfo(GetRootPath());
        var existingFileList = rootDirectory
            .GetFiles("*", SearchOption.AllDirectories)
            .Where(x => !IsFileIgnored(x.FullName))
            .ToArray();
        Parallel.ForEach(existingFileList, new ParallelOptions {
            MaxDegreeOfParallelism = ParallelismDegree
        }, file => {
            var relativePath = file.FullName.Replace(GetRootPath(), "");
            var cachedFile = _cachedFilesWithoutHash.FirstOrDefault(x => x.RelativePath == relativePath);
            if (cachedFile is not null) {
                var isLastUpdateSame = cachedFile.LastUpdate == file.LastWriteTimeUtc.Ticks;
                var isSizeSame = cachedFile.Size == file.Length;
                if (isLastUpdateSame && isSizeSame) {
                    Log.Verbose("File is same as cached {RelativeFilePath}", relativePath);
                    lock (files) {
                        files.Add(cachedFile);
                    }

                    return;
                }
            }

            Log.Verbose("Reading file {RelativeFilePath}", relativePath);
            var gameFile = new GameFileWithoutHash {
                RelativePath = relativePath,
                Size = file.Length,
                LastUpdate = file.LastWriteTimeUtc.Ticks
            };
            lock (files) {
                files.Add(gameFile);
            }
        });
        var filesDeletedFromLocal = _cachedFilesWithoutHash.Where(x => files.All(y => y.PathHash != x.PathHash)).ToList();
        var removedCount = _cachedFilesWithoutHash.RemoveAll(x => filesDeletedFromLocal.Any(y => y.PathHash == x.PathHash));
        var newFileCount = files.Count(x => _cachedFilesWithoutHash.All(y => y.PathHash != x.PathHash));
        Log.Debug("{RemovedFileCount}:{NewFileCount}:{FileCount}", removedCount, newFileCount, files.Count);
        _cachedFilesWithoutHash = files;
        return files;
    } // public List<GameFileWithoutHash> GetFilesWithoutHash() {

    public List<GameFile> GetFiles() {
        var files = new List<GameFile>();
        var rootDirectory = new DirectoryInfo(GetRootPath());
        var existingFileList = rootDirectory
            .GetFiles("*", SearchOption.AllDirectories)
            .Where(x => !IsFileIgnored(x.FullName))
            .ToArray();
        Parallel.ForEach(existingFileList, new ParallelOptions {
            MaxDegreeOfParallelism = ParallelismDegree
        }, file => {
            var relativePath = file.FullName.Replace(GetRootPath(), "");
            var cachedFile = _cachedFiles.FirstOrDefault(x => x.RelativePath == relativePath);
            if (cachedFile is not null) {
                var isLastUpdateSame = cachedFile.LastUpdate == file.LastWriteTimeUtc.Ticks;
                var isSizeSame = cachedFile.Size == file.Length;
                if (isLastUpdateSame && isSizeSame) {
                    Log.Verbose("File is same as cached {RelativeFilePath}", relativePath);
                    lock (files) {
                        files.Add(cachedFile);
                    }

                    return;
                }
            }

            Log.Verbose("Reading file {RelativeFilePath}", relativePath);
            var hash = HashManager.HashFileByFilePath(file.FullName)!;
            var gameFile = new GameFile {
                RelativePath = relativePath,
                Hash = hash,
                Size = file.Length,
                LastUpdate = file.LastWriteTimeUtc.Ticks
            };
            lock (files) {
                files.Add(gameFile);
            }
        });
        var filesDeletedFromLocal = _cachedFiles.Where(x => files.All(y => y.PathHash != x.PathHash)).ToList();
        var removedCount = _cachedFiles.RemoveAll(x => filesDeletedFromLocal.Any(y => y.PathHash == x.PathHash));
        var newFileCount = files.Count(x => _cachedFiles.All(y => y.PathHash != x.PathHash));
        Log.Debug("{RemovedFileCount}:{NewFileCount}:{FileCount}", removedCount, newFileCount, files.Count);
        _cachedFiles = files;
        return files;
    }

    public List<FileDiffResult> GetFileDiff(List<GameFile> localFiles, List<GameFile> remoteFiles) {
        var result = new List<FileDiffResult>();
        //Checking new or missing files in local
        foreach (var remoteFile in remoteFiles) {
            var localFile = localFiles.FirstOrDefault(x => x.PathHash == remoteFile.PathHash);
            result.Add(new FileDiffResult(remoteFile, localFile));
        }

        //Checking deleted files in remote 
        foreach (var localFile in localFiles) {
            var remoteFile = remoteFiles.FirstOrDefault(x => x.PathHash == localFile.PathHash);
            if (remoteFile is null) result.Add(new FileDiffResult(null, localFile));
        }

        return result;
    }

    public List<FileDiffResult> GetSomeFileDiff(List<GameFile> remoteFiles) {
        var list = new List<FileDiffResult>();
        foreach (var remoteFile in remoteFiles) {
            var path = GetFileFullPath(remoteFile.RelativePath);
            if (!File.Exists(path)) {
                list.Add(new FileDiffResult(remoteFile, null));
                continue;
            }

            var localFile = GetGameFileByFullPath(path);
            list.Add(new FileDiffResult(remoteFile, localFile));
        }

        return list;
    }

    public List<GameFile> GetSomeLocalFiles(List<GameFile> remoteFiles, out List<GameFile> notFoundFiles) {
        var list = new List<GameFile>();
        notFoundFiles = new List<GameFile>();
        foreach (var remoteFile in remoteFiles) {
            var path = GetFileFullPath(remoteFile.RelativePath);
            if (!File.Exists(path)) {
                notFoundFiles.Add(remoteFile);
                continue;
            }

            var localFile = GetGameFileByFullPath(path);
            list.Add(localFile);
        }

        return list;
    }

    public GameFile GetGameFileByFullPath(string path) {
        var fi = new FileInfo(path);
        var stream = fi.OpenRead();
        var hash = HashManager.HashFileByStream(stream);
        stream.Close();
        stream.Dispose();
        return new GameFile {
            RelativePath = path.Replace(GetRootPath(), ""),
            Hash = hash,
            Size = fi.Length,
            LastUpdate = fi.LastWriteTimeUtc.Ticks
        };
    }

    public GameFile GetGameFileByRelativePath(string path) {
        var fullPath = GetFileFullPath(path);
        return GetGameFileByFullPath(fullPath);
    }

    public GameFileWithoutHash? GetFileByPathHash(string hash) {
        var files = GetFilesWithoutHash();
        return files.FirstOrDefault(x => x.PathHash == hash);
    }

    public GameFile GetGameFileByFileInfo(FileInfo fileInfo) {
        var hash = HashManager.HashFileByFilePath(fileInfo.FullName);
        return new GameFile {
            RelativePath = fileInfo.FullName.Replace(GetRootPath(), ""),
            Hash = hash,
            Size = fileInfo.Length,
            LastUpdate = fileInfo.LastWriteTimeUtc.Ticks
        };
    }

    public string GetFileFullPath(string relativePath) {
        return Path.Combine(GetRootPath(), relativePath);
    }
}