using System;
using BierAlyzer.Contracts.Model;
using ProtoBuf;

namespace BierAlyzer.Contracts.Dto
{
    [ProtoContract]
    public class UserDto
    {
        [ProtoMember(10)]
        public Guid UserId { get; set; }

        [ProtoMember(20)]
        public string Username { get; set; }

        [ProtoMember(30)]
        public string Origin { get; set; }

        [ProtoMember(40)]
        public UserType Type { get; set; }

        [ProtoMember(50)]
        public string Mail { get; set; }

        [ProtoMember(60)]
        public string Hash { get; set; }

        [ProtoMember(70)]
        public string Salt { get; set; }

        [ProtoMember(80)]
        public bool Enabled { get; set; }

        [ProtoMember(90)]
        public double ConsumedLiters { get; set; }

        [ProtoMember(100)]
        public DateTime Created { get; set; }

        [ProtoMember(110)]
        public DateTime Modified { get; set; }

        [ProtoMember(120)]
        public DateTime LastLogin { get; set; }
    }
}
