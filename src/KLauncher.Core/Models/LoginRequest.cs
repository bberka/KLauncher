using KLauncher.Core.Manager;

namespace KLauncher.Core.Models;

public class LoginRequest
{
    public LoginRequest(string username, string password, bool rememberMe) {
        Username = username;
        Password = password;
        RememberMe = rememberMe;
        PasswordHash = HashManager.HashString(password);
    }

    public LoginRequest() {
    }

    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordHash { get; set; }
    public bool RememberMe { get; set; } = true;
}