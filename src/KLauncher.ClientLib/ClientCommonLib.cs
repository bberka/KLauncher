using Ardalis.Result;
using KLauncher.Core;
using KLauncher.Core.Models;
using KLauncher.Shared;

namespace KLauncher.ClientLib;

public static class ClientCommonLib
{
    public static Result<GameFile> Read(string fullFilePath) {
        var result = GameFile.Read(ClientConfigManager.This.Config.ClientFilesDirectoryPath, fullFilePath);
        if (result.Status != ResultStatus.Ok) return Result.Error(result.Errors.ToArray());
        return new GameFile() {
            RelativePath = result.Value.RelativePath,
            Hash = result.Value.Hash,
            Size = result.Value.Size,
            LastUpdate = result.Value.LastUpdate,
        };
    }
}