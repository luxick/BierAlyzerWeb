using System.Linq;
using Contracts.Model;
using Microsoft.AspNetCore.Http;

namespace BierAlyzerWeb.Helper
{
    public static class SessionHelper
    {
        #region GetUser

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Retrieves the user from session </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <param name="httpContext">  The httpContext to act on. </param>
        /// <returns>   The user. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static User GetUser(this HttpContext httpContext)
        {
            // Get User from session
            var sessionUser = httpContext?.Session?.GetObject<User>("User");
            if (sessionUser == null) return null;

            // Check if user needs to be updated
            if (!SharedProperties.OutdatedObjects.Contains(sessionUser.UserId)) return sessionUser;

            // If so, update session
            using (var context = ContextHelper.OpenContext())
            {
                SharedProperties.OutdatedObjects.RemoveAll(x => x.Equals(sessionUser.UserId));

                var contextUser = context.User.FirstOrDefault(u => u.UserId == sessionUser.UserId);
                if (contextUser == null)
                    return sessionUser;

                httpContext.Session.SetObject("User", contextUser);
            }

            return httpContext.Session?.GetObject<User>("User");
        }

        #endregion
    }
}