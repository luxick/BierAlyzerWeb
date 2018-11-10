using System;
using System.Collections.Generic;
using System.Text;
using Contracts.Interface.Communication;

namespace Contracts.Communication.Token.Request
{
    public class RefreshTokenRequest : IApiRequestParameter
    {
        public string RefreshToken { get; set; }
    }
}
