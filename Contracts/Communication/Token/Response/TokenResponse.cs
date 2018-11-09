using System;

namespace Contracts.Communication.Token.Response
{
    public class TokenResponse
    {
        public TokenResource AccessToken { get; set; }

        public TokenResource RefreshToken { get; set; }
    }
}
