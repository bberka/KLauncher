using Ardalis.Result.AspNetCore;
using KLauncher.ServerLib;
using KLauncher.ServerLib.Models;
using KLauncher.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace KLauncher.DownloadApi.Controllers;

[ApiController]
[Route(("api/[controller]/[action]"))]
public class LauncherController : ControllerBase
{
    private readonly ServerConfiguration _serverConfiguration;
    private readonly LauncherInformation _information;

    public LauncherController(
        IOptionsSnapshot<LauncherInformation> information,
        IOptions<ServerConfiguration> serverConfiguration) {
        _serverConfiguration = serverConfiguration.Value;
        _information = information.Value;
    }
    [HttpGet]
    public ActionResult<LauncherInformation> GetInformation() {
        return _information;
    }
    [HttpGet]
    public IActionResult Download() {
        var filePath = Path.Combine(_serverConfiguration.LauncherFilesDirectoryPath, _information.Version.ToString() + ".exe");
        var pathExist = System.IO.File.Exists(filePath);
        if (!pathExist) {
            Log.Warning("File {File} not found", filePath);
            return NotFound();
        }
        return File(System.IO.File.OpenRead(filePath), "application/octet-stream");
    }
}