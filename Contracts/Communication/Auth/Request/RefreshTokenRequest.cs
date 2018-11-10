using Contracts.Interface.Communication;

namespace Contracts.Communication.Auth.Request
{
    public class RefreshTokenRequest : IApiRequestParameter
    {
        public string RefreshToken { get; set; }
    }
}
