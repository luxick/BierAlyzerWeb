using Contracts.Model;
using System.Collections.Generic;

namespace BierAlyzerWeb.Models.Archive
{
    public class ArchiveModel
    {
        public ArchiveModel()
        {
            Events = new List<Event>();
        }

        public List<Event> Events { get; set; }

        public User User { get; set; }
    }
}
