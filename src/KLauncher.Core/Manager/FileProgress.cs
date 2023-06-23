using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using KLauncher.Core.Models;
using KLauncher.Shared;
using KLauncher.Shared.Abstract;

namespace KLauncher.Core.Manager;

public class FileProgress
{
    public GameFile File { get; }
    private readonly Stopwatch _stopwatch;
    private readonly PatchProgress _patchProgress;
    public int DownloadPercent => (int)(DownloadedBytes * 100 / File.Size);
    public long DownloadedBytes { get; set; } = 0;
    public bool IsFailed { get; private set; }
    private long _lastReceivedBytes = 0;
    public FileProgress(GameFile launcherFile, PatchProgress patchProgress)
    {
        File = launcherFile;
        _stopwatch = new();
        _patchProgress = patchProgress;
        _stopwatch.Start();

    }


    public void Failed()
    {
        _stopwatch.Stop();
        IsFailed = true;

    }
    public void Completed()
    {
        _stopwatch.Stop();
    }
    public void DownloadProgressCompletedEvent(object? sender, AsyncCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            Failed();
            _patchProgress.FailedDownload(File);
            return;
        }
        Completed();
        var speed = DownloadedBytes / _stopwatch.Elapsed.TotalSeconds;
        _patchProgress.CompletedDownload(File, _stopwatch.Elapsed.TotalSeconds);
    }

    public void DownloadProgressChangedEvent(object sender, DownloadProgressChangedEventArgs e)
    {
        var bytesReceived = e.BytesReceived - _lastReceivedBytes; //Getting the bytes downloaded for this progress event
        _lastReceivedBytes = bytesReceived;
        DownloadedBytes += bytesReceived;
        _patchProgress.BytesReceived(bytesReceived, File);
    }


}