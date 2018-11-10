using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Communication.Token
{
    public class TokenResource
    {
        public string Token { get; set; }

        public DateTime Expires { get; set; }
    }
}
