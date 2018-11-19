using System;
using BierAlyzer.Contracts.Model;
using ProtoBuf;

namespace BierAlyzer.Contracts.Dto
{
    [ProtoContract]
    public class EventDto
    {
        [ProtoMember(10)]
        public Guid EventId { get; set; }

        [ProtoMember(20)]
        public string Name { get; set; }

        [ProtoMember(30)]
        public string Description { get; set; }

        [ProtoMember(40)]
        public EventType Type { get; set; }

        [ProtoMember(50)]
        public string Code { get; set; }

        [ProtoMember(60)]
        public DateTime Start { get; set; }

        [ProtoMember(70)]
        public DateTime End { get; set; }

        [ProtoMember(80)]
        public EventStatus Status { get; set; }

        [ProtoMember(90)]
        public DateTime Created { get; set; }

        [ProtoMember(100)]
        public DateTime Modified { get; set; }

        [ProtoMember(110)]
        public Guid OwnerId { get; set; }
    }
}
