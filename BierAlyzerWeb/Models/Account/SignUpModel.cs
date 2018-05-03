using System.ComponentModel.DataAnnotations;
using BierAlyzerWeb.Helper;

namespace BierAlyzerWeb.Models.Account
{
    public class SignUpModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the name. </summary>
        /// <value> The name. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Du musst einen Namen angeben.")]
        public string Name { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the mail. </summary>
        /// <value> The mail. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Du musst eine E-Mail Adresse angeben.")]
        [ValidMail(ErrorMessage = "Das ist keine E-Mail Adresse.")]
        [MailNotRegistered(ErrorMessage = "E-Mail Adresse bereits registriert.")]
        public string Mail { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the origin. </summary>
        /// <value> The origin. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Du musst eine Universität angeben.")]
        public string Origin { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the password. </summary>
        /// <value> The password. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Du musst ein Passwort angeben.")]
        [StringLength(1000, MinimumLength = 6, ErrorMessage = "Das Passwort muss mindestens 6 Zeichen lang sein.")]
        public string Password { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the password confirmation. </summary>
        /// <value> The password confirmation. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Du musst ein Passwort angeben.")]
        [StringLength(1000, MinimumLength = 6, ErrorMessage = "Das Passwort muss mindestens 6 Zeichen lang sein.")]
        [Compare("Password", ErrorMessage = "Die eingegebenen Passwörter stimmen nicht überein.")]
        public string PasswordConfirmation { get; set; }
    }
}