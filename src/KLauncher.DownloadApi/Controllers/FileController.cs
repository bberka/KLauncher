using KLauncher.Core;
using KLauncher.Core.Manager;
using KLauncher.ServerLib;
using KLauncher.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KLauncher.DownloadApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly GameFileManager _gameFileManager;

        public FileController(GameFileManager gameFileManager) {
            _gameFileManager = gameFileManager;
        }
        // [HttpGet]
        // public IActionResult GetFile(string relativePath) {
        //     var fullPath = _gameFileManager.GetFileFullPath(relativePath);
        //     var pathExist = System.IO.File.Exists(fullPath);
        //     if (!pathExist) {
        //         Log.Warning("File {File} not found", relativePath);
        //         return NotFound();
        //     }
        //     return File(System.IO.File.OpenRead(fullPath), "application/octet-stream");
        // }
        [HttpGet]
        public IActionResult DownloadFile(string pathHash) {
            var file = _gameFileManager.GetFileByPathHash(pathHash);
            if (file is null) {
                Log.Warning("File with path hash {Hash} not found", pathHash);
                return NotFound();
            }
            var fullPath = _gameFileManager.GetFileFullPath(file.RelativePath);
            var pathExist = System.IO.File.Exists(fullPath);
            if (!pathExist) {
                Log.Warning("File {File} not found", file.RelativePath);
                return NotFound();
            }
            return File(System.IO.File.OpenRead(fullPath), "application/octet-stream");
        }
     
        [HttpGet]
        public ActionResult<List<GameFileWithoutHash>> GetFilesWithoutHash() {
            try {
                return _gameFileManager.GetFilesWithoutHash();
            }
            catch (Exception ex) {
                Log.Error(ex,"Exception occurred");
                return new List<GameFileWithoutHash>();
            }
        }
        [HttpGet]
        public ActionResult<List<GameFile>> GetFiles() {
            try {
                return _gameFileManager.GetFiles();
            }
            catch (Exception ex) {
                Log.Error(ex,"Exception occurred");
                return new List<GameFile>();
            }
        }
        
        
       
    }
}
