using KLauncher.Shared.Interface;
using Newtonsoft.Json;

namespace KLauncher.Core.Models;

public class ServerConfiguration : IServerConfiguration
{

    private ServerConfiguration() {
        var serverSettingFileExists = System.IO.File.Exists("server-config.json");
        if (!serverSettingFileExists) {
            throw new Exception("server-config.json does not exists");
        }
        var config = JsonConvert.DeserializeObject<ServerConfiguration>(System.IO.File.ReadAllText("server-config.json"));
        WhiteListIpAddress = config.WhiteListIpAddress;
        BlackList = config.BlackList;
        IsUseRateLimit = config.IsUseRateLimit;
        ServerStatusApiGetUrl = config.ServerStatusApiGetUrl;
        RememberMeTimeoutMinutes = config.RememberMeTimeoutMinutes;

    }
    public static ServerConfiguration This {
        get {
            _instance ??= new();
            return _instance;
        }
    }
    private static ServerConfiguration? _instance;
    /// <summary>
    /// Whitelist of allowed ip addresses. If empty, all ip addresses are allowed.
    /// </summary>
    public string[] WhiteListIpAddress { get; set; }
    /// <summary>
    /// Blacklist of blocked ip addresses. If empty, no ip addresses are blocked.
    /// </summary>
    public string[] BlackList { get;  }

    public bool IsUseRateLimit { get;  } = false;

    public string ServerStatusApiGetUrl { get; set; }
    /// <summary>
    /// Whether to timeout user after a specific time. If 0, no timeout.
    /// </summary>
    public int RememberMeTimeoutMinutes { get; set; }
}