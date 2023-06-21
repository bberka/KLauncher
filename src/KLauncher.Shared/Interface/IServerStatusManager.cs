using Ardalis.Result;
using Microsoft.AspNetCore.Hosting.Server;

namespace KLauncher.Shared.Interface;

public interface IServerStatusManager
{
    Result<IServerStatus> GetServerStatus();
}