using Contracts.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BierAlyzerWeb.Helper
{
    public static class Extensions
    {
        #region SetObject

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Writes an object to the session </summary>
        /// <remarks>   Andre Beging, 26.04.2018. </remarks>
        /// <param name="session">  The session to act on. </param>
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        #endregion

        #region GetObject

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets an object from the session </summary>
        /// <remarks>   Andre Beging, 26.04.2018. </remarks>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="session">  The session to act on. </param>
        /// <param name="key">      The key. </param>
        /// <returns>   The object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        #endregion

        #region IsSignedIn

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Check if the session holds a valid user </summary>
        /// <remarks>   Andre Beging, 26.04.2018. </remarks>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>   True if signed in, false if not. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool IsSignedIn(this HttpContext httpContext)
        {
            var user = httpContext.GetUser();
            return user != null;
        }

        #endregion

        #region CheckUserType

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Check if the users type is one of the given types </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="httpContext">  HttpContext. </param>
        /// <param name="types">        A variable-length parameters list containing types. </param>
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool CheckUserType(this HttpContext httpContext, params UserType[] types)
        {
            var user = httpContext?.GetUser();
            if (user == null) return false;

            foreach (var userType in types)
                if (user.Type == userType) return true;

            return false;
        }

        #endregion
    }
}