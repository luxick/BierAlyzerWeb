using Contracts.Communication.Auth.Request;

namespace Contracts.Interface.Service
{
    public interface IAuthController<T>
    {
        T Token(TokenRequest request);

        T Refresh(RefreshTokenRequest refreshToken);
    }
}
