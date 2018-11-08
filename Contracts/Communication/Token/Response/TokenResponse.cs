﻿using System;

namespace Contracts.Communication.Token.Response
{
    public class TokenResponse
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   API Token </summary>
        ///
        /// <value> The token. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Token { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Timestamp of when the token will expire </summary>
        ///
        /// <value> The expires. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DateTime Expires { get; set; }
    }
}