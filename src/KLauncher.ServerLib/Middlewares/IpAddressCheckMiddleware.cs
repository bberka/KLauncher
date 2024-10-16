using KLauncher.ServerLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace KLauncher.ServerLib.Middlewares;

public class IpAddressCheckMiddleware : IMiddleware
{
  private readonly DownloadServerConfiguration _downloadServerConfiguration;

  public IpAddressCheckMiddleware(IOptions<DownloadServerConfiguration> options) {
    _downloadServerConfiguration = options.Value;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
    var remoteIpAddress = context.Connection.RemoteIpAddress?.ToString()!;
    if (!string.IsNullOrEmpty(_downloadServerConfiguration.RealIpAddressHeader)) remoteIpAddress = context.Request.Headers[_downloadServerConfiguration.RealIpAddressHeader].ToString()!;
    // var isBlocked = IsBlocked(remoteIpAddress);
    var isAllowed = IsAllowed(remoteIpAddress);
    if (!isAllowed) context.Response.StatusCode = StatusCodes.Status403Forbidden;
    await next(context);
  }

  private bool IsAllowed(string ipAddress) {
    var isCheckWhiteList = _downloadServerConfiguration.AllowIpAddress is not null && _downloadServerConfiguration.AllowIpAddress.Length == 0;
    if (!isCheckWhiteList) return false;
    return _downloadServerConfiguration.AllowIpAddress!.Any(x => x == ipAddress || x == "*");
  }
  // private bool IsBlocked(string ipAddress) {
  //     var isCheckBlackList = _serverConfiguration.BlackListIpAddress is not null && _serverConfiguration.BlackListIpAddress.Length == 0;
  //     if (!isCheckBlackList) return false;
  //     return _serverConfiguration.BlackListIpAddress!.Any(x => x == ipAddress|| x == "*");
  // }
}