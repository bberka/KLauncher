using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace KLauncher.Core.Manager;

public class FileProgress
{
    private readonly PatchProgress _patchProgress;
    private readonly Stopwatch _stopwatch;
    private long _lastReceivedBytes;

    public FileProgress(GameFile launcherFile, PatchProgress patchProgress) {
        File = launcherFile;
        _stopwatch = new Stopwatch();
        _patchProgress = patchProgress;
        _stopwatch.Start();
    }

    public GameFile File { get; }
    public int DownloadPercent => (int)(DownloadedBytes * 100 / File.Size);
    public long DownloadedBytes { get; set; }
    public bool IsFailed { get; private set; }


    public void Failed() {
        _stopwatch.Stop();
        IsFailed = true;
    }

    public void Completed() {
        _stopwatch.Stop();
    }

    public void DownloadProgressCompletedEvent(object? sender, AsyncCompletedEventArgs e) {
        if (e.Cancelled) {
            Failed();
            _patchProgress.FailedDownload(File);
            return;
        }

        Completed();
        var speed = DownloadedBytes / _stopwatch.Elapsed.TotalSeconds;
        _patchProgress.CompletedDownload(File, _stopwatch.Elapsed.TotalSeconds);
    }

    public void DownloadProgressChangedEvent(object sender, DownloadProgressChangedEventArgs e) {
        var bytesReceived = e.BytesReceived - _lastReceivedBytes; //Getting the bytes downloaded for this progress event
        _lastReceivedBytes = bytesReceived;
        DownloadedBytes += bytesReceived;
        _patchProgress.BytesReceived(bytesReceived, File);
    }
}