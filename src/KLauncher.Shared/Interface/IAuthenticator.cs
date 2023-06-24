using EasMe.Result;

namespace KLauncher.Shared.Interface;

public interface IAuthenticator
{
    ResultData<IUser> Login(string username, string password, bool rememberMe);
    ResultData<IUser> LoginWithPlayToken(string playToken);
    ResultData<IUser> LoginWithRefreshToken(string refreshToken);
}