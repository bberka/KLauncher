using EasMe.Result;

namespace KLauncher.Shared.Interface;

public interface IServerStatusManager
{
  ResultData<IServerStatus> GetServerStatus();
}