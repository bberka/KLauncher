﻿using KLauncher.Core.Manager;

namespace KLauncher.Core;

public class GameFileWithoutHash
{
    /// <summary>
    ///     File relative path from root directory.
    /// </summary>
    public string RelativePath { get; init; }

    /// <summary>
    ///     Relative path hash string to compare files in same location and name. All / and \ are replaced with * to avoid path
    ///     issues.
    ///     This value is used to make sure the file path is the same not the actual content.
    /// </summary>
    public string PathHash => HashManager.HashAsHexString(
        RelativePath
            .Replace("\\", "*")
            .Replace("/", "*"));

    /// <summary>
    ///     File size in bytes. Max file size is 1GB.
    /// </summary>
    public long Size { get; init; }

    /// <summary>
    ///     Last file update time in ticks (UTC).
    /// </summary>
    public long LastUpdate { get; init; }

    public bool IsOlderThan(GameFileWithoutHash gameFileWithoutHash) {
        return LastUpdate < gameFileWithoutHash.LastUpdate;
    }
}