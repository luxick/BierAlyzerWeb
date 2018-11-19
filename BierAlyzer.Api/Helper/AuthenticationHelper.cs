using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BierAlyzer.Api.Helper
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An authentication helper. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        #region GenerateAccessToken

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates an access token. </summary>
        /// <remarks>   Andre Beging, 09.11.2018. </remarks>
        /// <param name="configuration">    The configuration. </param>
        /// <param name="userClaims">       The user claims. </param>
        /// <returns>   The access token. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static JwtSecurityToken GenerateAccessToken(IConfiguration configuration, Claim[] userClaims)
        {
            return GenerateToken(configuration, userClaims, GenerateTokenType.Access);
        }

        #endregion GenerateAccessToken

        #region GenerateRefreshToken

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a refresh token. </summary>
        /// <remarks>   Andre Beging, 09.11.2018. </remarks>
        /// <param name="configuration">    The configuration. </param>
        /// <param name="userClaims">       The user claims. </param>
        /// <returns>   The refresh token. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static JwtSecurityToken GenerateRefreshToken(IConfiguration configuration, Claim[] userClaims)
        {
            return GenerateToken(configuration, userClaims, GenerateTokenType.Refresh);
        }

        #endregion GenerateRefreshToken

        #region ValidateLifetime

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validates the lifetime of a token </summary>
        /// <remarks>   Andre Beging, 09.11.2018. </remarks>
        /// <param name="notbefore">            The notbefore. </param>
        /// <param name="expires">              The expires. </param>
        /// <param name="securitytoken">        The securitytoken. </param>
        /// <param name="validationparameters"> The validationparameters. </param>
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool ValidateLifetime(DateTime? notbefore, DateTime? expires, SecurityToken securitytoken, TokenValidationParameters validationparameters)
        {
            return !(expires < DateTime.UtcNow);
        }

        #endregion ValidateLifetime

        #region Private functions

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a token. </summary>
        /// <remarks>   Andre Beging, 09.11.2018. </remarks>
        /// <param name="configuration">    The configuration. </param>
        /// <param name="userClaims">       The user claims. </param>
        /// <param name="type">             The type. </param>
        /// <returns>   The token. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static JwtSecurityToken GenerateToken(IConfiguration configuration, Claim[] userClaims, GenerateTokenType type)
        {
            var jwtConfiguration = configuration.GetSection("Jwt");

            int lifeTime;
            switch (type)
            {
                case GenerateTokenType.Access:
                    lifeTime = jwtConfiguration.GetValue<int>("TokenLifetime");
                    break;
                case GenerateTokenType.Refresh:
                    lifeTime = jwtConfiguration.GetValue<int>("RefreshTokenLifetime");
                    break;
                default:
                    return null;
            }

            var expireDate = DateTime.UtcNow.AddSeconds(lifeTime);

            var issuer = jwtConfiguration.GetValue<string>("Issuer");
            var audience = jwtConfiguration.GetValue<string>("Audience");
            var secret = jwtConfiguration.GetValue<string>("Key");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                audience: audience,
                issuer: issuer,
                claims: userClaims,
                expires: expireDate,
                signingCredentials: creds);
        }

        #endregion Private functions

        private enum GenerateTokenType
        {
            Access,
            Refresh
        }
    }
}
