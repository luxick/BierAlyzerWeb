using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contracts.Model
{
    public class DrinkEntry
    {
        [Key]
        public Guid EntryId { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid EventId { get; set; }

        public Event Event { get; set; }

        public Guid DrinkId { get; set; }

        public Drink Drink { get; set; }
    }
}
