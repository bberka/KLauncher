namespace KLauncher.Shared.Interface;

public interface IDownloadServerConfiguration : IBaseServerConfiguration
{
    public string[] AlwaysCheckFiles { get; }
    public string ClientFilesDirectoryPath { get; set; }
    public string LauncherFilesDirectoryPath { get; set; }
}