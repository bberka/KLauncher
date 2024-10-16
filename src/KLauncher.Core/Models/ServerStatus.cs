using KLauncher.Shared.Enum;
using KLauncher.Shared.Interface;

namespace KLauncher.Core.Models;

public class ServerStatus : IServerStatus
{
  public int OnlinePlayerCount { get; set; }
  public ServerStatusType Status { get; set; }
}