using System.ComponentModel.DataAnnotations;
using KLauncher.Shared.Interface;
using KLauncher.Shared.Validators;

namespace KLauncher.ServerLib.Models;

public class ServerConfiguration : IServerConfiguration
{
    public const string SectionName = "ServerConfiguration";

    public string[] AllowIpAddress { get; set; }
    
    public string[] AlwaysCheckFiles { get; set; }
    
    public bool IsUseRateLimit { get; set; }
    
    public string ServerStatusApiGetUrl { get; set;  }
    
    [DirectoryPathValidator]
    public string ClientFilesDirectoryPath { get; set; }
    
    [DirectoryPathValidator]
    public string LauncherFilesDirectoryPath { get; set; }
    public string RealIpAddressHeader { get; set; }
    

}