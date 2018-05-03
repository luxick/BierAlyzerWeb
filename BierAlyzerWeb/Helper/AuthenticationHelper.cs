using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BierAlyzerWeb.Models;
using BierAlyzerWeb.Models.Account;
using Contracts.Model;

namespace BierAlyzerWeb.Helper
{
    public static class AuthenticationHelper
    {
        #region GenerateSalt

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a salt. </summary>
        /// <remarks>   Andre Beging, 26.04.2018. </remarks>
        /// <returns>   The salt. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string GenerateSalt()
        {
            return GetHash(Guid.NewGuid().ToString());
        }

        #endregion

        #region GetHash

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates a hash from a plain text </summary>
        /// <remarks>   Andre Beging, 26.04.2018. </remarks>
        /// <param name="text"> The text. </param>
        /// <returns>   The hash. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string GetHash(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) text = Guid.Empty.ToString();

            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(text);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var t in hashBytes)
                    sb.Append(t.ToString("X2"));
                return sb.ToString();
            }
        }

        #endregion

        #region CalculatePasswordHash

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Calculates the password hash. </summary>
        /// <remarks>   Andre Beging, 26.04.2018. </remarks>
        /// <param name="salt">     The salt. </param>
        /// <param name="password"> The password. </param>
        /// <returns>   The calculated password hash. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string CalculatePasswordHash(string salt, string password)
        {
            var passwordHashString = string.Format("{0}{1}{0}", salt, password);
            return GetHash(passwordHashString);
        }

        #endregion

        #region TrySignUp

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Attempts to sign up from the given data. </summary>
        /// <remarks>   Andre Beging, 26.04.2018. </remarks>
        /// <param name="model">    The model. </param>
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool TrySignUp(SignUpModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password)) return false;
            if (string.IsNullOrWhiteSpace(model.PasswordConfirmation)) return false;
            if (model.Password != model.PasswordConfirmation) return false;
            if (string.IsNullOrWhiteSpace(model.Mail)) return false;

            using (var context = ContextHelper.OpenContext())
            {
                if (context.User.Any(u => u.Mail.Trim().ToLower() == model.Mail.Trim().ToLower()))
                    return false;

                // Data valid

                var salt = GenerateSalt();
                var hash = CalculatePasswordHash(salt, model.Password);

                var newUser = new User
                {
                    Mail = model.Mail.Trim(),
                    Username = model.Name,
                    Origin = model.Origin.Trim(),
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Salt = salt,
                    Hash = hash,
                    Type = UserType.User,
                    Enabled = true
                };

                context.User.Add(newUser);
                var result = context.SaveChanges();

                // No rows affected?
                if (result == 0) return false;

                // All good
                return true;
            }
        }

        #endregion

        #region LoginCorrect

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validates the login data </summary>
        /// <remarks>   Andre Beging, 26.04.2018. </remarks>
        /// <param name="mail">     The mail. </param>
        /// <param name="password"> The password. </param>
        /// <param name="user">The matching user</param>
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool LoginCorrect(string mail, string password, out User user)
        {
            user = null;

            using (var context = ContextHelper.OpenContext())
            {
                var contextUser = context.User.FirstOrDefault(u => u.Mail.ToLower() == mail.ToLower());
                if (contextUser == null) return false;

                var salt = contextUser.Salt;
                var hash = CalculatePasswordHash(salt, password);

                if (hash == contextUser.Hash)
                {
                    user = contextUser;
                    return true;
                }
                return false;
            }
        }

        #endregion
    }
}