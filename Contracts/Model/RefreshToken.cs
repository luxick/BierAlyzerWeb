using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contracts.Model
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
