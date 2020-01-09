using System.Linq;

namespace BierAlyzer.Web.Helper
{
    public class EventHelper
    {
        #region GenerateCode

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Generates a new event code </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <returns>   The code. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string GenerateCode()
        {
            var code = AuthenticationHelper.GenerateSalt().Substring(0, 4).ToLower();

            using (var context = ContextHelper.OpenContext())
            {
                if (context.Event.Any(e => e.Code.ToLower() == code))
                    return GenerateCode();
            }

            return code;
        }

        #endregion
    }
}