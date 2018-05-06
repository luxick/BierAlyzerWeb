using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Model;

namespace BierAlyzerWeb.Models.Management
{
    public class ManageUserModel
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Kein Name angegeben.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Keine Universität angegeben.")]
        public string Origin { get; set; }

        public string Mail { get; set; }

        [Required]
        public UserType Type { get; set; }

        [Required(ErrorMessage = "Kein Passwort angegeben.")]
        [StringLength(1000, MinimumLength = 6, ErrorMessage = "Das Passwort muss mindestens 6 Zeichen lang sein.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Kein Passwort angegeben.")]
        [StringLength(1000, MinimumLength = 6, ErrorMessage = "Das Passwort muss mindestens 6 Zeichen lang sein.")]
        [Compare("Password", ErrorMessage = "Die eingegebenen Passwörter stimmen nicht überein.")]
        public string PasswordConfirmation { get; set; }
    }
}