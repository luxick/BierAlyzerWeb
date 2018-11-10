using Contracts.Communication.Token.Request;

namespace Contracts.Interface.Service
{
    public interface IAuthController<T>
    {
        T Token(TokenRequest request);

        T Refresh(RefreshTokenRequest refreshToken);
    }
}
