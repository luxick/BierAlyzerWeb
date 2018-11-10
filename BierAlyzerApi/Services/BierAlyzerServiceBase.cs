using BierAlyzerApi.Models;

namespace BierAlyzerApi.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A bier alyzer service base. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BierAlyzerServiceBase
    {
        /// <summary>   The context. </summary>
        protected readonly BierAlyzerContext Context;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="context">  The context. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public BierAlyzerServiceBase(BierAlyzerContext context)
        {
            Context = context;
        }
    }
}
