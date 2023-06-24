using KLauncher.Shared.Interface;

namespace KLauncher.Core.Manager;

public class AuthenticationManager : IAuthenticator
{
    public ResultData<IUser> Login(string username, string password, bool rememberMe) {
        throw new NotImplementedException();
    }

    public ResultData<IUser> LoginWithPlayToken(string playToken) {
        throw new NotImplementedException();
    }

    public ResultData<IUser> LoginWithRefreshToken(string refreshToken) {
        throw new NotImplementedException();
    }
}