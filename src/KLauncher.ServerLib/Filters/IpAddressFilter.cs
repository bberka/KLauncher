using KLauncher.Core.Manager;
using KLauncher.ServerLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace KLauncher.ServerLib.Filters;

public class IpAddressFilter : IActionFilter
{
    private readonly DownloadServerConfiguration _downloadServerConfiguration;

    public IpAddressFilter(IOptions<DownloadServerConfiguration> options) {
        _downloadServerConfiguration = options.Value;
    }

    public void OnActionExecuting(ActionExecutingContext context) {
        var httpContext = context.HttpContext;
        var remoteIpAddress = httpContext.Connection.RemoteIpAddress?.ToString()!;
        if (!string.IsNullOrEmpty(_downloadServerConfiguration.RealIpAddressHeader)) {
            var realIpAddress = httpContext.Request.Headers[_downloadServerConfiguration.RealIpAddressHeader].ToString()!;
            if (!string.IsNullOrEmpty(realIpAddress)) remoteIpAddress = realIpAddress;
        }

        // var isBlocked = IsBlocked(remoteIpAddress);
        var isAllowed = IsAllowed(remoteIpAddress);
        if (!isAllowed) context.Result = new UnauthorizedResult();
        // httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
    }

    public void OnActionExecuted(ActionExecutedContext context) {
    }

    private bool IsAllowed(string ipAddress) {
        if (ConstManager.IsDevelopment) return true;
        var isCheckWhiteList = _downloadServerConfiguration.AllowIpAddress is not null || _downloadServerConfiguration.AllowIpAddress.Length == 0;
        if (!isCheckWhiteList) return false;
        return _downloadServerConfiguration.AllowIpAddress!.Any(x => x == ipAddress || x == "*");
    }
}