using KLauncher.ClientLib.Models;
using Newtonsoft.Json;

namespace KLauncher.ClientLib;

public class ClientConfigManager
{
    private static ClientConfigManager? _instance;

    private ClientConfigManager() {
        var serverSettingFileExists = File.Exists("server-config.json");
        if (!serverSettingFileExists)
            throw new Exception("client-config.json does not exists");
        var text = File.ReadAllText("client-config.json");
        Config = JsonConvert.DeserializeObject<ClientConfiguration>(text);
        if (Config is null) throw new Exception("server-config.json is empty");
        if (!Directory.Exists(Config.ClientFilesDirectoryPath))
            throw new Exception("ClientFilesDirectoryPath does not exists");
    }

    public static ClientConfigManager This {
        get {
            _instance ??= new ClientConfigManager();
            return _instance;
        }
    }

    public ClientConfiguration Config { get; }

    public static void Read() {
        _ = This.Config; //Access to init constructor
    }
}