using AutoMapper;

namespace BierAlyzer.Api.Models
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A bier alyzer service base. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BierAlyzerServiceBase
    {
        /// <summary>   Database context </summary>
        protected readonly BierAlyzerContext Context;

        /// <summary>   Automapper </summary>
        protected readonly IMapper Mapper;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="context">  Database context </param>
        /// <param name="mapper">   Automapper </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public BierAlyzerServiceBase(BierAlyzerContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
    }
}
