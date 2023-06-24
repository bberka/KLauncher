namespace KLauncher.Core.Models;

public class RefreshRequest
{
    public RefreshRequest(string token) {
        Token = token;
    }

    public RefreshRequest() {
    }

    public string Token { get; set; }
}