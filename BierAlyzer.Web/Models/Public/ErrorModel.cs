using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BierAlyzerWeb.Models.Public
{
    public class ErrorModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
