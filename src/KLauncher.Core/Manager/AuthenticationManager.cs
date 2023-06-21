using Ardalis.Result;
using KLauncher.Shared.Interface;

namespace KLauncher.Core.Manager;

public class AuthenticationManager : IAuthenticator
{
    public Result<IUser> Login(string username, string password) {
        throw new NotImplementedException();
    }

    public Result<IUser> LoginWithPlayToken(string playToken) {
        throw new NotImplementedException();
    }

    public Result<IUser> LoginWithRefreshToken(string refreshToken) {
        throw new NotImplementedException();
    }
}