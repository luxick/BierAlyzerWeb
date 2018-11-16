

using System.Collections.Generic;

namespace Contracts.Communication.Event
{
    public class EventResponse
    {
        public int EventCount => Events.Count;

        public List<Model.Event> Events { get; set; }
    }
}
