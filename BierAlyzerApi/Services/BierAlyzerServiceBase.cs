using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BierAlyzerApi.Models;

namespace BierAlyzerApi.Services
{
    public class BierAlyzerServiceBase
    {
        protected readonly BierAlyzerContext Context;

        public BierAlyzerServiceBase(BierAlyzerContext context)
        {
            Context = context;
        }
    }
}
