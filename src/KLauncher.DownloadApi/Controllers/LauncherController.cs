using KLauncher.ServerLib.Models;
using KLauncher.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace KLauncher.DownloadApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class LauncherController : ControllerBase
{
    private readonly DownloadServerConfiguration _downloadServerConfiguration;
    private readonly LauncherInformation _information;

    public LauncherController(
        IOptionsSnapshot<LauncherInformation> information,
        IOptions<DownloadServerConfiguration> serverConfiguration) {
        _downloadServerConfiguration = serverConfiguration.Value;
        _information = information.Value;
    }

    [HttpGet]
    public ActionResult<LauncherInformation> GetInformation() {
        return _information;
    }

    [HttpGet]
    public IActionResult Download() {
        try {
            var filePath = Path.Combine(_downloadServerConfiguration.LauncherFilesDirectoryPath, _information.Version + ".exe");
            var pathExist = System.IO.File.Exists(filePath);
            if (!pathExist) {
                Log.Warning("File {File} not found", filePath);
                return NotFound();
            }

            return File(System.IO.File.OpenRead(filePath), "application/octet-stream");
        }
        catch (Exception ex) {
            Log.Error(ex, "Exception occurred");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}