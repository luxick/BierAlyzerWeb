using System;
using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.EntityModel
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Represents the database entity of a drink entry </summary>
    /// <remarks>   Andre Beging, 17.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
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
