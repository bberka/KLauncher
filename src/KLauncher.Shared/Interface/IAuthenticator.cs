using Ardalis.Result;

namespace KLauncher.Shared.Interface;

public interface IAuthenticator
{
    Result<IUser> Login(string username, string password); 
    Result<IUser> LoginWithPlayToken(string playToken);
    Result<IUser> LoginWithRefreshToken(string refreshToken);
}