using System.Configuration;
using System.Reflection;
using KLauncher.Core.Models;
using Newtonsoft.Json;

namespace KLauncher.Core;


public class ConstantManager
{

    private ConstantManager() {
       var serverSettingFileExists = System.IO.File.Exists("server-config.json");
       if (serverSettingFileExists) {
           ServerConfig = JsonConvert.DeserializeObject<ServerConfiguration>(System.IO.File.ReadAllText("server-config.json"));
       }

    }
    public static ConstantManager This {
        get {
            _instance ??= new();
            return _instance;
        }
    }
    private static ConstantManager? _instance;

    public ServerConfiguration ServerConfig { get;  }
    public string IpBlockList { get;  }
    public string LauncherName { get;  }
    public string LauncherVersion { get; }
    public string ServerName { get; }
    public string WebsiteDomain { get; }
    public string DownloadServerDomain { get; }
    public string AuthServerDomain { get; }
    public string StatusServerDomain { get; }
    public string GetStatusUrl { get; }
}