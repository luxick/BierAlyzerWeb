using System;
using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.EntityModel
{
    public class RefreshToken
    {
        [Key]
        public Guid TokenId { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

    }
}
