namespace KLauncher.Shared.Interface;

public interface IServerConfiguration
{
    /// <summary>
    /// Whitelist of allowed ip addresses. If empty, all ip addresses are allowed.
    /// </summary>
    public string[] AllowIpAddress { get; } 

    public string[] AlwaysCheckFiles { get; }
    public bool IsUseRateLimit { get; }
    public string ServerStatusApiGetUrl { get; }
    public string ClientFilesDirectoryPath { get; set; }
    public string LauncherFilesDirectoryPath { get; set; }
    
    public string RealIpAddressHeader { get; set; }


}