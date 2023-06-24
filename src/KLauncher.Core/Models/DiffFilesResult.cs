using KLauncher.Shared.Enum;

namespace KLauncher.Core.Models;

public class FileDiffResult
{
    public FileDiffResult(GameFile? remoteFile, GameFile? localFile) {
        RemoteFile = remoteFile;
        LocalFile = localFile;
        ThrowIfRelativePathNotMatch();
        UpdateGameFileStatus();
    }

    public GameFileStatus Status { get; private set; }

    public GameFile? RemoteFile { get; }
    public GameFile? LocalFile { get; }

    private void ThrowIfRelativePathNotMatch() {
        if (RemoteFile is null || LocalFile is null) return;
        if (RemoteFile.PathHash != LocalFile.PathHash) throw new Exception("Relative path not match");
    }

    private void UpdateGameFileStatus() {
        Status = _get();

        GameFileStatus _get() {
            if (RemoteFile is null && LocalFile is null) return GameFileStatus.Same;
            if (RemoteFile is null && LocalFile is not null) return GameFileStatus.Deleted;
            if (RemoteFile is not null && LocalFile is null) return GameFileStatus.NewOrMissing;
            if (RemoteFile is not null && LocalFile is not null) {
                if (RemoteFile.Hash == LocalFile.Hash) return GameFileStatus.Same;

                return GameFileStatus.Different;
            }

            throw new Exception("Unknown status");
        }
    }
}