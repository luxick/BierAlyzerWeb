using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BierAlyzer.Contracts.Model;

namespace BierAlyzer.Api
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An extensions. </summary>
    /// <remarks>   Andre Beging, 18.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class Extensions
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Tries to get the value of an claim
        /// </summary>
        /// <remarks>   Andre Beging, 18.11.2018. </remarks>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="claims">       The claims to act on. </param>
        /// <param name="claimType">    Type of the the desired claim. </param>
        /// <param name="value">        [out] The value. </param>
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool TryGetValue<T>(this IEnumerable<Claim> claims, string claimType, out T value)
        {
            value = default(T);

            var stringValue = claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            if (stringValue == null) return false;

            var tType = typeof(T);
            switch (tType.Name)
            {
                // Get Guid
                case "Guid":
                    if (Guid.TryParse(stringValue, out var guidValue))
                        value = (T) (object) guidValue;
                    else
                        return false;
                    break;
                // Get UserType
                case "UserType":
                    if (Enum.TryParse(stringValue, out UserType userTypeValue))
                        value = (T) (object) userTypeValue;
                    else
                        return false;
                    break;
                // Get string
                case "String":
                    value = (T) (object) stringValue;
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}
