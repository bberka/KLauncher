using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLauncher.DownloadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        [HttpGet("{file}")]
        public IActionResult GetFile(string file) {
            return File(System.IO.File.OpenRead(file), "application/octet-stream");
        }

        [HttpGet]
        public IActionResult List() {

        }
    }
}
