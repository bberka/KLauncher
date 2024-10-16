using EasMe.Result;
using KLauncher.Core.Models;
using KLauncher.Shared.Interface;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KLauncher.AuthenticationApi.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly IAuthenticator _authenticator;

    public AuthController(IAuthenticator authenticator) {
        _authenticator = authenticator;
    }

    [HttpPost]
    public ActionResult<ResultData<IUser>> Login(LoginRequest request) {
        try {
            var res = _authenticator.Login(request.Username, request.Password, request.RememberMe);
            if (res.IsSuccess)
                Log.Information("Login successful {UserId} {UserName}", res.Data.UserId, res.Data.Username);
            else
                Log.Warning("Login failed {UserName} {Message}", request.Username, res.Errors.FirstOrDefault());

            return Ok(res);
        }
        catch (Exception ex) {
            Log.Error(ex, "Exception occurred");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public ActionResult<ResultData<IUser>> Refresh(RefreshRequest request) {
        try {
            var res = _authenticator.LoginWithRefreshToken(request.Token);
            if (res.IsSuccess)
                Log.Information("Refresh successful {UserId} {UserName}", res.Data.UserId, res.Data.Username);
            else
                Log.Warning("Refresh failed {Token} {Message}", request.Token, res.Errors.FirstOrDefault());
            return Ok(res);
        }
        catch (Exception ex) {
            Log.Error(ex, "Exception occurred");
            return StatusCode(500);
        }
    }
}