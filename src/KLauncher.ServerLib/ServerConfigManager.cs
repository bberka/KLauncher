using KLauncher.ServerLib.Models;
using KLauncher.Shared.Interface;
using KLauncher.Shared.Models;
using Newtonsoft.Json;

namespace KLauncher.ServerLib;

public class ServerConfigManager
{
    private ServerConfigManager() {
        var serverSettingFileExists = File.Exists("server-config.json");
        if (!serverSettingFileExists) 
            throw new Exception("server-config.json does not exists");
        var text = File.ReadAllText("server-config.json");
        Config = JsonConvert.DeserializeObject<ServerConfiguration>(text);
        if (Config is null) throw new Exception("server-config.json is empty");
        if (!Directory.Exists(Config.LauncherFilesDirectoryPath))
            throw new Exception("LauncherFilesDirectoryPath does not exists");
        if (!Directory.Exists(Config.ClientFilesDirectoryPath))
            throw new Exception("ClientFilesDirectoryPath does not exists");
        // var parseVersion = Version.TryParse(Config.LauncherVersion, out var version);
        // if (!parseVersion || version is null) throw new Exception("LauncherVersion is not valid");
        // GameFileManager.SetRootPath(Config.ClientFilesDirectoryPath);
    }
    public static ServerConfigManager This {
        get {
            _instance ??= new();
            return _instance;
        }
    }
    private static ServerConfigManager? _instance;
    public ServerConfiguration Config { get; private set; }

    public static void Read() {
        _ = This.Config; //Access to init constructor
    }
}