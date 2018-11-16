using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using BierAlyzer.Api.Models;
using BierAlyzer.EntityModel;
using BierAlyzerApi.Helper;

namespace BierAlyzer.Api.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An authentication service. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class AuthService : BierAlyzerServiceBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="context">  The context. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public AuthService(BierAlyzerContext context) : base(context)
        {
        }

        #region ValidateCredentials

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validate user credentials </summary>
        ///
        /// <remarks>   Andre Beging, 17.06.2018. </remarks>
        ///
        /// <param name="mail">     The mail. </param>
        /// <param name="password"> The password. </param>
        ///
        /// <returns>   True if credentials are correct. False is not </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Claim[] ValidateCredentials(string mail, string password)
        {
            var contextUser = Context.User.FirstOrDefault(x => x.Mail.Equals(mail, StringComparison.InvariantCultureIgnoreCase) && x.Enabled);
            if (contextUser == null) return null;

            var hash = AuthenticationHelper.CalculatePasswordHash(contextUser.Salt, password);

            if (hash.Equals(contextUser.Hash, StringComparison.InvariantCulture))
            {
                return new[]
                {
                    new Claim(ClaimTypes.UserData, contextUser.UserId.ToString()),
                    new Claim(ClaimTypes.Locality, "de-DE")
                };
            }

            return null;
        }

        #endregion ValidateCredentials

        #region ValidateRefreshToken

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validates the refresh token described by refreshToken. </summary>
        /// <remarks>   Andre Beging, 09.11.2018. </remarks>
        /// <param name="refreshToken"> The refresh token. </param>
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Claim[] ValidateRefreshToken(JwtSecurityToken refreshToken)
        {
            var userIdString = refreshToken?.Claims?.First(x => x.Type == ClaimTypes.UserData).Value;
            var userId = Guid.Parse(userIdString);
            if (userId == Guid.Empty) return null;

            // Check in database
            var databaseToken = Context.RefreshToken.FirstOrDefault(x =>
                x.Token == refreshToken.RawData &&
                x.Expires == refreshToken.ValidTo && x.UserId == userId);
            if (databaseToken == null) return null;

            // Check user exists
            var user = Context.User.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return null;

            // Remove old refresh token
            Context.RefreshToken.Remove(databaseToken);
            Context.SaveChanges();

            return new[]
            {
                new Claim(ClaimTypes.UserData, userId.ToString()),
                new Claim(ClaimTypes.Locality, "de-DE")
            };
        }

        #endregion ValidateRefreshToken

        #region StoreRefreshToken

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Stores a refresh token into the database </summary>
        /// <remarks>   Andre Beging, 09.11.2018. </remarks>
        /// <param name="refreshToken"> The refresh token. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool StoreRefreshToken(JwtSecurityToken refreshToken)
        {
            // Validate token existence
            if (refreshToken == null) return false;
            if (refreshToken.Claims.All(x => x.Type != ClaimTypes.UserData)) return false;

            var userIdString = refreshToken.Claims.First(x => x.Type == ClaimTypes.UserData).Value;

            if (Guid.TryParse(userIdString, out var userId))
            {
                // Validate existing userId
                if (!Context.User.Any(u => u.UserId == userId)) return false;

                Context.RefreshToken.Add(new RefreshToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                    Expires = refreshToken.ValidTo,
                    UserId = userId
                });

                // Save refresh token to database
                var saveResult = Context.SaveChanges();
                if (saveResult > 0) return true;
            }

            return false;
        }

        #endregion StoreRefreshToken
    }
}
