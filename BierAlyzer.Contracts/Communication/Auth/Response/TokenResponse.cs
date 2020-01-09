using BierAlyzer.Contracts.Interface.Communication;
using BierAlyzer.Contracts.Model;
using ProtoBuf;

namespace BierAlyzer.Contracts.Communication.Auth.Response
{
    [ProtoContract]
    public class TokenResponse : IApiResponseParameter
    {
        [ProtoMember(10)]
        public TokenResource AccessToken { get; set; }

        [ProtoMember(20)]
        public TokenResource RefreshToken { get; set; }

        [ProtoMember(30)]
        public RequestResult Result { get; set; }
    }
}
