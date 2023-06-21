using KLauncher.Shared.Enum;

namespace KLauncher.Shared.Interface;

public interface IServerStatus
{
    public int OnlinePlayerCount { get; set; }
    public ServerStatusType Status { get; set; }
}