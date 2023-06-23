using Ardalis.Result;
using KLauncher.Core.Models;
using KLauncher.Shared;
using KLauncher.Shared.Abstract;
using KLauncher.Shared.Interface;

namespace KLauncher.Core.Manager;

public class PatchManager
{
    public Result<ICollection<GameFile>> GetUpdateList() {
        throw new NotImplementedException();
    }

    public Result DownloadPatch(ICollection<GameFile> files) {
        throw new NotImplementedException();
    }
}