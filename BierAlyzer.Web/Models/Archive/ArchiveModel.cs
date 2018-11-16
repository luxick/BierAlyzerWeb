using System.Collections.Generic;
using BierAlyzer.EntityModel;

namespace BierAlyzer.Web.Models.Archive
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
