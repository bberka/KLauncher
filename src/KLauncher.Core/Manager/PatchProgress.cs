using System.Diagnostics;
using KLauncher.Core.Models;
using KLauncher.Shared;
using KLauncher.Shared.Abstract;

namespace KLauncher.Core.Manager;

public delegate void PatchProgressChanged(PatchProgress patchProgress);  // delegate
public delegate void PatchProgressCompleted();  // delegate
//public delegate void PatchFileDownloadCompleted

public class PatchProgress
{
    public const int MAX_PARALLEL_DOWNLOAD_COUNT = 5;

    public const int RETRY_FAIL_DOWNLOAD_COUNT = 3;

    /// <summary>
    /// Whether the patch is a single zip file or multiple files.  Only false value is supported.
    /// </summary>
    public const bool IS_PATCH_ZIP = false;


    /// <summary>
    /// Whether to cache patch details to a local file and skip file check for next startup.
    /// If false launcher will read patch files every startup.
    /// This is recommended to be true for low end devices. Setting this value false will increase startup time.
    /// </summary>
    public const bool IS_USE_UPDATE_CACHING_CLIENT = true;
    /// <summary>
    /// Whether to use server and client version check. If false, launcher will check patches by file size and last modified date.
    /// </summary>
    public const bool IS_USE_SERVER_VERSION = true;

    /// <summary>
    /// Whether to use file hash check. If false, launcher will check patches by file size and last modified date.
    /// </summary>
    public const bool IS_USE_FILE_HASH_ONLY = true;

    /// <summary>
    /// If this value is changed, BytesReceived method should be updated.
    /// </summary>
    public const int DOWNLOAD_SPEED_UPDATE_MILISECONDS = 1000;

    public const string DOWNLOADING_BASE_TEXT = "({0}% - {1}/{2}) @ {3} MB/s {4}";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="totalBytes"></param>
    /// <param name="totalFileCount"></param>
    public PatchProgress(List<GameFile> launcherFiles)
    {
        _launcherFiles = launcherFiles;
        _totalBytes = launcherFiles.Sum(x => x.Size);
        _totalFileCount = launcherFiles.Count;
        _stopWatch.Start();
    }

    private readonly List<GameFile> _launcherFiles;
    private readonly long _totalBytes;
    private readonly int _totalFileCount;

    private readonly Stopwatch _stopWatchForSpeedCalculation = new Stopwatch();
    private readonly Stopwatch _stopWatch = new Stopwatch();

    private string _lastByteReceivedFileName = string.Empty;
    private long _downloadedBytesForSpeedCalculation = 0;

    private long _downloadedBytes = 0;
    private int _downloadedFileCount = 0;

    public int DownloadPercent => (int)(_downloadedBytes * 100 / _totalBytes);
    public bool IsFinished => _downloadedFileCount == _totalFileCount;
    public bool IsCancelled { get; private set; } = false;
    public int FailCount { get; private set; } = 0;
    public double DownloadSpeed { get; private set; } = 0.0;

    public string DownloadInformationText => string.Format(DOWNLOADING_BASE_TEXT, DownloadPercent, _downloadedFileCount, _totalFileCount, DownloadSpeed.ToString("0.0"), _lastByteReceivedFileName);
    //
    //public List<FileProgress> CompletedFileProgresses { get; set; } = new();
    //public List<FileProgress> CancelledFileProgresses { get; set; } = new();
    //public List<FileProgress> FailedFileProgresses { get; set; } = new();

    //

    public event PatchProgressChanged PatchProgressChanged;
    public event PatchProgressCompleted PatchProgressCompleted;


    /// <summary>
    /// Download speed calculation is done here. This method should be called every time a download byte received called.
    /// </summary>
    /// <param name="bytesReceived"></param>
    /// <param name="launcherFile"></param>
    public void BytesReceived(long bytesReceived, GameFile launcherFile)
    {
        PatchProgressChanged?.Invoke(this);
        _downloadedBytes += bytesReceived;
        _downloadedBytesForSpeedCalculation += bytesReceived;
        if (_stopWatchForSpeedCalculation.IsRunning == false) _stopWatchForSpeedCalculation.Start();
        if (!(_stopWatchForSpeedCalculation.Elapsed.TotalMilliseconds >= DOWNLOAD_SPEED_UPDATE_MILISECONDS)) return;
        DownloadSpeed = _downloadedBytesForSpeedCalculation / 1024.0 / 1024.0 / _stopWatchForSpeedCalculation.Elapsed.TotalSeconds * (1000 / DOWNLOAD_SPEED_UPDATE_MILISECONDS);
        _lastByteReceivedFileName = Path.GetFileName(launcherFile.RelativePath);
        _stopWatchForSpeedCalculation.Reset();
        _stopWatchForSpeedCalculation.Start();
        _downloadedBytesForSpeedCalculation = 0;
    }

    public void FailedDownload(GameFile fileProgress)
    {

    }

    protected internal virtual void CompletedDownload(GameFile fileProgress, double completedInSeconds/*,double speed*/)
    {
        _downloadedFileCount++;

        if (IsFinished)
        {
            PatchProgressCompleted?.Invoke();
        }

        //TODO: logging


    }
}