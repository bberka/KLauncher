using KLauncher.Shared.Interface;
using KLauncher.Shared.Validators;

namespace KLauncher.ServerLib.Models;

public class DownloadServerConfiguration : IDownloadServerConfiguration
{
    public const string SectionName = "DownloadServerConfiguration";
    public string[] AllowIpAddress { get; set; }

    public string[] AlwaysCheckFiles { get; set; }


    [DirectoryPathValidator]
    public string ClientFilesDirectoryPath { get; set; }

    [DirectoryPathValidator]
    public string LauncherFilesDirectoryPath { get; set; }

    public string RealIpAddressHeader { get; set; }
}