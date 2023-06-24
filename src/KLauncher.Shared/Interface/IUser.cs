namespace KLauncher.Shared.Interface;

public interface IUser
{
    string UserId { get; }
    string Username { get; }
    string EmailAddress { get; }
    string PlayToken { get; }
    string? RefreshToken { get; }
    int Cash { get; }
    DateTime LastLoginDate { get; }
}