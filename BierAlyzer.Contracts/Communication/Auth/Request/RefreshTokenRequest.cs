using BierAlyzer.Contracts.Interface.Communication;
using ProtoBuf;

namespace BierAlyzer.Contracts.Communication.Auth.Request
{
    [ProtoContract]
    public class RefreshTokenRequest : IApiRequestParameter
    {
        [ProtoMember(1)]
        public string RefreshToken { get; set; }
    }
}
