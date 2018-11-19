using System;
using ProtoBuf;

namespace BierAlyzer.Contracts.Communication.Auth
{
    [ProtoContract]
    public class TokenResource
    {
        [ProtoMember(10)]
        public string Token { get; set; }

        [ProtoMember(20)]
        public DateTime Expires { get; set; }
    }
}
