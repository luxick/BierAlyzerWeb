using System;
using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.EntityModel
{
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