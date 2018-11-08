using System;
using System.Security.Cryptography;
using System.Text;

namespace BierAlyzerApi.Helper
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
    }
}
