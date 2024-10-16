using KLauncher.Core.Manager;
using KLauncher.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace KLauncher.ServerLib.Middlewares;

public class UserAgentCheckMiddleware : IMiddleware
{
  private readonly EncryptionManager _encryptionManager;
  private readonly LauncherInformation _options;

  public UserAgentCheckMiddleware(IOptionsSnapshot<LauncherInformation> options, EncryptionManager encryptionManager) {
    _encryptionManager = encryptionManager;
    _options = options.Value;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
    var userAgent = context.Request.Headers["User-Agent"].ToString();
    var decryptedUserAgent = _encryptionManager.Decrypt(userAgent);
    if (decryptedUserAgent != _options.Name) context.Response.StatusCode = StatusCodes.Status403Forbidden;
    await next(context);
  }
}