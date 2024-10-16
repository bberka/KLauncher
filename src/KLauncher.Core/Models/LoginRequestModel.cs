using KLauncher.Core.Manager;

namespace KLauncher.Core.Models;

public sealed record class LoginRequestModel(string Username, string Password, string PasswordHash, bool RememberMe);