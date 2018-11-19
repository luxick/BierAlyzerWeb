using System;
using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.EntityModel
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Represents the database entity of the connection between an event and an user </summary>
    /// <remarks>   Andre Beging, 17.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class UserEvent
    {
        [Key]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Key]
        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}