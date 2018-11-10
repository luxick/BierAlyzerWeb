using Contracts.Interface.Communication;
using ProtoBuf;

namespace Contracts.Communication.Auth.Response
{
    [ProtoContract]
    public class TokenResponse : IApiResponseParameter
    {
        [ProtoMember(1)]
        public TokenResource AccessToken { get; set; }

        [ProtoMember(2)]
        public TokenResource RefreshToken { get; set; }
    }
}
