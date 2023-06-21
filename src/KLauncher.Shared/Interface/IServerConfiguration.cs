namespace KLauncher.Shared.Interface;

public interface IServerConfiguration
{
    /// <summary>
    /// Whitelist of allowed ip addresses. If empty, all ip addresses are allowed.
    /// </summary>
    public string[] WhiteListIpAddress { get; } 
    /// <summary>
    /// Blacklist of blocked ip addresses. If empty, no ip addresses are blocked.
    /// </summary>
    public string[] BlackList { get; } 

    public bool IsUseRateLimit { get; } 

    public string ServerStatusApiGetUrl { get; }
    /// <summary>
    /// Whether to timeout user after a specific time. If 0, no timeout.
    /// </summary>
    public int RememberMeTimeoutMinutes { get; }
}