using System;
using ProtoBuf;

namespace BierAlyzer.Contracts.Communication.Auth
{
    [ProtoContract]
    public class TokenResource
    {
        [ProtoMember(1)]
        public string Token { get; set; }

        [ProtoMember(2)]
        public DateTime Expires { get; set; }
    }
}
