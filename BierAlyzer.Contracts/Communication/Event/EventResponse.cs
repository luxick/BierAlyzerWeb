

using System.Collections.Generic;
using BierAlyzer.Contracts.Dto;
using BierAlyzer.Contracts.Interface.Communication;
using BierAlyzer.Contracts.Model;
using ProtoBuf;

namespace BierAlyzer.Contracts.Communication.Event
{
    [ProtoContract]
    public class EventResponse : IApiResponseParameter
    {
        [ProtoMember(10)]
        public int EventCount => Events.Count;

        [ProtoMember(20)]
        public List<EventDto> Events { get; set; }

        [ProtoMember(30)]
        public RequestResult Result { get; set; }

        public EventResponse()
        {
            Events = new List<EventDto>();
            Result = new RequestResult();
        }
    }
}
