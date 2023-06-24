using KLauncher.Core.Manager;
using KLauncher.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace KLauncher.ServerLib.Filters;

public class UserAgentFilter : IActionFilter
{
    private readonly EncryptionManager _encryptionManager;
    private readonly LauncherInformation _options;

    public UserAgentFilter(IOptionsSnapshot<LauncherInformation> options) {
        _encryptionManager = new EncryptionManager();
        _options = options.Value;
    }

    public void OnActionExecuting(ActionExecutingContext context) {
        if (ConstManager.IsDevelopment) return;
        var httpContext = context.HttpContext;
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        var decryptedUserAgent = _encryptionManager.Decrypt(userAgent);
        if (decryptedUserAgent != _options.Name) context.Result = new UnauthorizedResult();
    }

    public void OnActionExecuted(ActionExecutedContext context) {
    }
}