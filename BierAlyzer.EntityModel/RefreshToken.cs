using System;
using System.ComponentModel.DataAnnotations;

namespace BierAlyzer.EntityModel
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Represents the database entity of a refresh token </summary>
    /// <remarks>   Andre Beging, 17.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class RefreshToken
    {
        [Key]
        public Guid TokenId { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

    }
}
