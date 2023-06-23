using Ardalis.Result;
using KLauncher.Core;
using KLauncher.Core.Models;
using KLauncher.Shared;

namespace KLauncher.ServerLib.Models;

public class ServerFile : GameFile
{
    public static Result<ServerFile> Read(string fullFilePath) {
        var result = GameFile.Read(ServerConfigManager.This.Config.ClientFilesDirectoryPath, fullFilePath);
        if (result.Status != ResultStatus.Ok) return Result.Error(result.Errors.ToArray());
        return new ServerFile() {
            RelativePath = result.Value.RelativePath,
            Hash = result.Value.Hash,
            Size = result.Value.Size,
            LastUpdate = result.Value.LastUpdate,
        };
    }
}